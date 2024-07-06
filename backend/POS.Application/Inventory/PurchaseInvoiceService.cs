using Microsoft.EntityFrameworkCore;
using POS.Application.Abstractions;
using POS.Application.Abstractions.Data;
using POS.Core.Inventory;

namespace POS.Application.Inventory;

public sealed class PurchaseInvoiceService : IPurchaseInvoiceService
{
    private readonly ITransactionManager _transactionManager;
    private readonly IRepository<PurchaseInvoice> _repository;
    private readonly IRepository<PurchaseInvoiceItem> _itemRepository;
    private readonly IGeneralService _generalService;
    private readonly IVendorService _vendorService;
    private readonly IItemService _itemService;
    private readonly IPricingService _pricingService;

    public PurchaseInvoiceService(IRepository<PurchaseInvoice> repository, ITransactionManager transactionManager, IRepository<PurchaseInvoiceItem> itemRepository, IGeneralService generalService, IVendorService vendorService, IItemService itemService, IPricingService pricingService)
    {
        _repository = repository;
        _transactionManager = transactionManager;
        _itemRepository = itemRepository;
        _generalService = generalService;
        _vendorService = vendorService;
        _itemService = itemService;
        _pricingService = pricingService;
    }

    public async Task<PurchaseInvoice> Create(CreatePurchaseInvoiceRequest createRequest, string userId)
    {
        using var t = await _transactionManager.BeginTransactionAsync();
        var invoice = await MapToInvoice(createRequest, userId);

        _repository.Add(invoice);
        await _repository.SaveChangesAsync();

        await t.CommitAsync();
        return invoice;
    }

    private async Task<PurchaseInvoice> MapToInvoice(CreatePurchaseInvoiceRequest invoice, string userId)
    {
        ArgumentNullException.ThrowIfNull(invoice);
        ArgumentNullException.ThrowIfNull(invoice.InvoiceItems);

        await CheckDateAndNumber(invoice.Date, invoice.Number);
        CheckRowNumbers(invoice.InvoiceItems);

        var result = new PurchaseInvoice
        {
            Date = new DateTime(invoice.Date.Year, invoice.Date.Month, invoice.Date.Day, invoice.Date.Hour, invoice.Date.Minute, invoice.Date.Second),
            Number = invoice.Number ?? await GetMaxNumber() + 1,
            Description = invoice.Description ?? "",
            Vendor = await GetVendor(invoice.VendorId),
            CreatorID = userId,
            InvoiceItems = new List<PurchaseInvoiceItem>(invoice.InvoiceItems.Count)
        };

        if(await _pricingService.IsThereAnyPricingInDate(result.Date))
            throw new POSException("there is a pricing in this date");

        var total = 0m;
        foreach (var item in invoice.InvoiceItems)
        {
            if (item.Price <= 0)
                throw new POSException("invoice item price should be positive");

            if (item.Quantity <= 0)
                throw new POSException("invoice item quantity should be positive");

            result.InvoiceItems.Add(new PurchaseInvoiceItem
            {
                RowNumber = item.RowNumber,
                Quantity = item.Quantity,
                Fee = Math.Round(item.Price / item.Quantity, 4),
                Price = item.Price,
                Item = await GetItem(item.ItemId)
            });

            total += item.Price;
        }
        result.TotalPrice = total;
        return result;
    }

    public async Task<PurchaseInvoice> Update(UpdatePurchaseInvoiceRequest updateRequest)
    {
        using var t = await _transactionManager.BeginTransactionAsync();
        var invoice = await MapToInvoice(updateRequest);

        _repository.Update(invoice);
        await _repository.SaveChangesAsync();

        await _itemService.ValidateItemQuantity();

        await t.CommitAsync();
        return invoice;
    }

    private async Task<PurchaseInvoice> MapToInvoice(UpdatePurchaseInvoiceRequest invoice)
    {
        ArgumentNullException.ThrowIfNull(invoice);
        ArgumentNullException.ThrowIfNull(invoice.InvoiceItems);

        var previousInvoice = _repository.Set.Include(i => i.Vendor).Include(i => i.InvoiceItems).ThenInclude(ii => ii.Item).FirstOrDefault(i => i.ID == invoice.ID);
        if (previousInvoice is null)
            throw new POSException("invalid id");

        previousInvoice.Number = invoice.Number ?? await GetMaxNumber() + 1;
        previousInvoice.Description = invoice.Description ?? "";
        previousInvoice.Date = new DateTime(invoice.Date.Year, invoice.Date.Month, invoice.Date.Day, invoice.Date.Hour, invoice.Date.Minute, invoice.Date.Second);

        if(await _pricingService.IsThereAnyPricingInDate(previousInvoice.Date))
            throw new POSException("there is a pricing in this date");

        await CheckDateAndNumber(previousInvoice.Date, previousInvoice.Number, previousInvoice.ID);
        CheckRowNumbers(invoice.InvoiceItems);

        if (previousInvoice.Vendor.ID != (invoice.VendorId ?? -1))
            previousInvoice.Vendor = await GetVendor(invoice.VendorId);

        for (int i = previousInvoice.InvoiceItems.Count - 1; i >= 0; i--)
        {
            if (!invoice.InvoiceItems.Any(ii => ii.ID == previousInvoice.InvoiceItems[i].ID))
                previousInvoice.InvoiceItems.RemoveAt(i);
        }

        var total = 0m;
        foreach (var item in invoice.InvoiceItems)
        {
            if (item.Price <= 0)
                throw new POSException("invoice item price should be positive");

            if (item.Quantity <= 0)
                throw new POSException("invoice item quantity should be positive");

            total += item.Price;

            var previousItem = item.ID is null
                ? null
                : previousInvoice.InvoiceItems.FirstOrDefault(ii => ii.ID == item.ID);

            if (previousItem is null)
            {
                previousInvoice.InvoiceItems.Add(new PurchaseInvoiceItem
                {
                    RowNumber = item.RowNumber,
                    Quantity = item.Quantity,
                    Fee = Math.Round(item.Price / item.Quantity, 4),
                    Price = item.Price,
                    Item = await GetItem(item.ItemId)
                });
            }
            else
            {
                previousItem.RowNumber = item.RowNumber;
                previousItem.Quantity = item.Quantity;
                previousItem.Fee = Math.Round(item.Price / item.Quantity, 4);
                previousItem.Price = item.Price;
                previousItem.Item = await GetItem(item.ItemId);
            }
        }
        previousInvoice.TotalPrice = total;
        return previousInvoice;
    }

    private async Task CheckDateAndNumber(DateTime date, int? number, int excludedInvoiceId = -1)
    {
        if (date < (await _generalService.GetStoreInfo()).InitializationDate)
            throw new POSException("invoice date is before initialization date of the store");

        if (number is not null && await _repository.Set.AnyAsync(i => (excludedInvoiceId == -1 || i.ID != excludedInvoiceId) && i.Number == number))
            throw new POSException("invoice number already exists");
    }

    private void CheckRowNumbers(List<CreatePurchaseInvoiceItemRequest> invoiceItems)
    {
        Span<bool> rowNumbers = stackalloc bool[invoiceItems.Count];
        for (int i = 0; i < invoiceItems.Count; i++)
        {
            if (invoiceItems[i].RowNumber > invoiceItems.Count
                || invoiceItems[i].RowNumber < 1
                || rowNumbers[invoiceItems[i].RowNumber - 1])
            {
                throw new POSException("wrong row numbers in invoice");
            }

            rowNumbers[invoiceItems[i].RowNumber - 1] = true;
        }
    }

    private void CheckRowNumbers(List<UpdatePurchaseInvoiceItemRequest> invoiceItems)
    {
        Span<bool> rowNumbers = stackalloc bool[invoiceItems.Count];
        for (int i = 0; i < invoiceItems.Count; i++)
        {
            if (invoiceItems[i].RowNumber > invoiceItems.Count
                || invoiceItems[i].RowNumber < 1
                || rowNumbers[invoiceItems[i].RowNumber - 1])
            {
                throw new POSException("wrong row numbers in invoice");
            }

            rowNumbers[invoiceItems[i].RowNumber - 1] = true;
        }
    }

    private async Task<Item> GetItem(int itemId)
    {
        var item = await _itemService.GetByID(itemId, true);
        if (item is null)
            throw new POSException("item not found");

        return item;
    }

    private async Task<Vendor> GetVendor(int? vendorId)
    {
        var v = await _vendorService.GetByID(vendorId ?? -1, true);
        if (v is null)
            throw new POSException("vendor not found");

        return v;
    }

    private async ValueTask<int> GetMaxNumber()
    {
        return await _repository.Set.MaxAsync(i => (int?)i.Number) ?? 1;
    }

    public Task<PurchaseInvoice?> GetByID(int id)
    {
        return _repository.Set.Where(i => i.ID == id).Include(i => i.Vendor).Include(i => i.InvoiceItems).ThenInclude(ii => ii.Item).AsNoTracking().FirstOrDefaultAsync();
    }

    public Task<List<PurchaseInvoice>> GetAll(int? page, int? pageSize)
    {
        if (page is null || pageSize is null || page < 0 || pageSize < 1)
            return _repository.Set.AsNoTracking().ToListAsync();

        return _repository.Set.Skip(page.Value * pageSize.Value).Take(pageSize.Value).Include(i => i.Vendor).AsNoTracking().ToListAsync();
    }

    public Task<int> GetCount()
    {
        return _repository.Set.CountAsync();
    }

    public async Task Remove(int id)
    {
        using var t = await _transactionManager.BeginTransactionAsync();
        
        var invoice = await GetByID(id);
        if(invoice is null)
            throw new POSException("invalid id");

        if(await _pricingService.IsThereAnyPricingInDate(invoice.Date))
            throw new POSException("there is a pricing in this date");

        foreach(var item in invoice.InvoiceItems)
        {
            _itemRepository.Remove(item.ID);
        }

        _repository.Remove(id);
        await _repository.SaveChangesAsync();

        await _itemService.ValidateItemQuantity();
        
        await t.CommitAsync();
    }
}
