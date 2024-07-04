using Microsoft.EntityFrameworkCore;
using POS.Application.Abstractions;
using POS.Application.Abstractions.Data;
using POS.Core.Inventory;
using POS.Core.Sales;

namespace POS.Application.Sales;

public sealed class SaleInvoiceService : ISaleInvoiceService
{
    private readonly ITransactionManager _transactionManager;
    private readonly IRepository<SaleInvoice> _repository;
    private readonly IRepository<SaleInvoiceItem> _itemRepository;
    private readonly IGeneralService _generalService;
    private readonly ICustomerService _customerService;
    private readonly IItemService _itemService;

    public SaleInvoiceService(IRepository<SaleInvoice> repository, ITransactionManager transactionManager, IRepository<SaleInvoiceItem> itemRepository, IGeneralService generalService, ICustomerService customerService, IItemService itemService)
    {
        _repository = repository;
        _transactionManager = transactionManager;
        _itemRepository = itemRepository;
        _generalService = generalService;
        _customerService = customerService;
        _itemService = itemService;
    }

    public async Task<SaleInvoice> Create(CreateSaleInvoiceRequest createRequest, string userId)
    {
        using var t = await _transactionManager.BeginTransactionAsync();
        var invoice = await MapToInvoice(createRequest, userId);

        _repository.Add(invoice);
        await _repository.SaveChangesAsync();

        await _itemService.ValidateItemQuantity();

        await t.CommitAsync();
        return invoice;
    }

    public async Task<SaleInvoice> Update(UpdateSaleInvoiceRequest updateRequest)
    {
        using var t = await _transactionManager.BeginTransactionAsync();
        var invoice = await MapToInvoice(updateRequest);

        _repository.Update(invoice);
        await _repository.SaveChangesAsync();

        await _itemService.ValidateItemQuantity();

        await t.CommitAsync();
        return invoice;
    }

    private async Task<SaleInvoice> MapToInvoice(UpdateSaleInvoiceRequest invoice)
    {
        ArgumentNullException.ThrowIfNull(invoice);
        ArgumentNullException.ThrowIfNull(invoice.InvoiceItems);

        var previousInvoice = _repository.Set.Include(i => i.Customer).Include(i => i.InvoiceItems).ThenInclude(ii => ii.Item).FirstOrDefault(i => i.ID == invoice.ID);
        if (previousInvoice is null)
            throw new POSException("invalid id");

        previousInvoice.Number = invoice.Number ?? await GetMaxNumber() + 1;
        previousInvoice.Date = invoice.Date;

        await CheckDateAndNumber(previousInvoice.Date, previousInvoice.Number, previousInvoice.ID);
        CheckRowNumbers(invoice.InvoiceItems);

        if (previousInvoice.Customer.ID != (invoice.CustomerId ?? -1))
            previousInvoice.Customer = await GetCustomer(invoice.CustomerId);

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
                previousInvoice.InvoiceItems.Add(new SaleInvoiceItem
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

    private async Task<SaleInvoice> MapToInvoice(CreateSaleInvoiceRequest invoice, string userId)
    {
        ArgumentNullException.ThrowIfNull(invoice);
        ArgumentNullException.ThrowIfNull(invoice.InvoiceItems);

        await CheckDateAndNumber(invoice.Date, invoice.Number);
        CheckRowNumbers(invoice.InvoiceItems);

        var result = new SaleInvoice
        {
            Date = invoice.Date,
            Number = invoice.Number ?? await GetMaxNumber() + 1,
            Customer = await GetCustomer(invoice.CustomerId),
            CreatorID = userId,
            InvoiceItems = new List<SaleInvoiceItem>(invoice.InvoiceItems.Count)
        };

        var total = 0m;
        foreach (var item in invoice.InvoiceItems)
        {
            if (item.Price <= 0)
                throw new POSException("invoice item price should be positive");

            if (item.Quantity <= 0)
                throw new POSException("invoice item quantity should be positive");

            result.InvoiceItems.Add(new SaleInvoiceItem
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

    private async Task CheckDateAndNumber(DateTime date, int? number, int excludedInvoiceId = -1)
    {
        if (date < (await _generalService.GetStoreInfo()).InitializationDate)
            throw new POSException("invoice date is before initialization date of the store");

        if (number is not null && await _repository.Set.AnyAsync(i => (excludedInvoiceId == -1 || i.ID != excludedInvoiceId) && i.Number == number))
            throw new POSException("invoice number already exists");
    }

    private void CheckRowNumbers(List<CreateSaleInvoiceItemRequest> invoiceItems)
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

    private void CheckRowNumbers(List<UpdateSaleInvoiceItemRequest> invoiceItems)
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

    private async Task<Customer> GetCustomer(int? customerId)
    {
        var v = await _customerService.GetByID(customerId ?? -1, true);
        if (v is null)
            throw new POSException("customer not found");

        return v;
    }

    private async ValueTask<int> GetMaxNumber()
    {
        return await _repository.Set.MaxAsync(i => (int?)i.Number) ?? 1;
    }

    public Task<SaleInvoice?> GetByID(int id)
    {
        return _repository.Set.Where(i => i.ID == id).Include(i => i.Customer).Include(i => i.InvoiceItems).ThenInclude(ii => ii.Item).AsNoTracking().FirstOrDefaultAsync();
    }

    public Task<List<SaleInvoice>> GetAll(int? page, int? pageSize)
    {
        if (page is null || pageSize is null || page < 0 || pageSize < 1)
            return _repository.Set.AsNoTracking().ToListAsync();

        return _repository.Set.Skip(page.Value * pageSize.Value).Take(pageSize.Value).Include(i => i.Customer).AsNoTracking().ToListAsync();
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

        foreach(var item in invoice.InvoiceItems)
        {
            _itemRepository.Remove(item.ID);
        }

        _repository.Remove(id);
        await _repository.SaveChangesAsync();
        
        await t.CommitAsync();
    }
}
