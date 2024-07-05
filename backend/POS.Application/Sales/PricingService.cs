using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using POS.Application.Abstractions;
using POS.Application.Abstractions.Data;
using POS.Core.Sales;

namespace POS.Application.Sales;

public sealed class PricingService : IPricingService
{
    private readonly IRepository<Pricing> _repository;
    private readonly IGeneralService _generalService;
    private readonly ITransactionManager _transactionManager;

    public PricingService(IRepository<Pricing> repository, IGeneralService generalService, ITransactionManager transactionManager)
    {
        _repository = repository;
        _generalService = generalService;
        _transactionManager = transactionManager;
    }

    private enum PricingRelatedInvoiceType
    {
        Purchase = 0,
        Sale = 1
    }

    private sealed record PricingRelatedInvoice(int ItemID, DateTime InvoiceDate, PricingRelatedInvoiceType InvoiceType, int InvoiceItemID, decimal InvoiceItemQuantity, decimal InvoiceItemPrice);
    public async Task DoPricing(DateTime startDate, DateTime endDate)
    {
        using var t = await _transactionManager.BeginTransactionAsync();

        if (startDate != await GetNewPricingStartDate())
            throw new POSException("pricing start date is wrong");

        endDate = endDate.Date.AddDays(1).AddSeconds(-1);
        if (endDate <= startDate)
            throw new POSException("end date should be after start date in pricing");

        var relatedInvoiceItems = await _repository.ExecuteRawSql<PricingRelatedInvoice>($"""
DECLARE @StartDate DATETIME = {startDate};
DECLARE @EndDate DATETIME = {endDate};

WITH InvoiceItems AS (
    SELECT PI.ItemID, CAST(P.[Date] AS Date) AS InvoiceDate, 0 AS InvoiceType, PI.ID AS InvoiceItemID, P.Number as InvoiceNumber, PI.Quantity AS InvoiceItemQuantity, PI.Price AS InvoiceItemPrice
    FROM PurchaseInvoiceItem PI
    INNER JOIN PurchaseInvoices P ON PI.PurchaseInvoiceID = P.ID

    UNION ALL

    SELECT SI.ItemID, CAST(S.[Date] AS Date) AS InvoiceDate, 1 AS InvoiceType, SI.ID AS InvoiceItemID, S.Number as InvoiceNumber, SI.Quantity AS InvoiceItemQuantity, SI.Price AS InvoiceItemPrice
    FROM SaleInvoiceItem SI
    INNER JOIN SaleInvoices S ON SI.SaleInvoiceID = S.ID
)
SELECT *
FROM InvoiceItems
ORDER BY InvoiceItems.ItemID ASC, InvoiceItems.InvoiceDate ASC, InvoiceItems.InvoiceType ASC, InvoiceItems.InvoiceNumber ASC
""");
        List<InvoiceAveragePurchasePrice> averagePrices = [];
        CalculatePrices(relatedInvoiceItems, averagePrices, startDate);

        _repository.Add(new Pricing
        {
            StartDate = startDate,
            EndDate = endDate,
            InvoiceAveragePurchasePrices = averagePrices
        });

        await _repository.SaveChangesAsync();
        await t.CommitAsync();
    }

    public Task<List<Pricing>> GetAll(int? page, int? pageSize)
    {
        if (page is null || pageSize is null || page < 0 || pageSize < 1)
            return _repository.Set.AsNoTracking().ToListAsync();

        return _repository.Set.Skip(page.Value * pageSize.Value).Take(pageSize.Value).AsNoTracking().ToListAsync();
    }

    public Task<int> GetCount()
    {
        return _repository.Set.CountAsync();
    }

    public async Task Remove(int id)
    {
        using var t = await _transactionManager.BeginTransactionAsync();

        if((await _repository.Set.AsNoTracking().FirstOrDefaultAsync(p => p.ID == id)) is null)
            throw new POSException("invalid id");

        _ = _repository.ExecuteRawSql<int>($"""
DELETE FROM InvoiceAveragePurchasePrice WHERE PricingID = {id}
""");
        _repository.Remove(id);
        await _repository.SaveChangesAsync();
        await t.CommitAsync();
    }

    private static void CalculatePrices(List<PricingRelatedInvoice> relatedInvoicesList, List<InvoiceAveragePurchasePrice> averagePrices, DateTime startDate)
    {
        var relatedItems = CollectionsMarshal.AsSpan(relatedInvoicesList);
        var relatedItemsCount = relatedItems.Length;

        var itemID = -1;
        var totalPrice = 0m;
        var totalQuantity = 0m;
        var average = 0m;

        for (int i = 0; i < relatedItemsCount; i++)
        {
            ref var ri = ref relatedItems[i];
            if (itemID != ri.ItemID)
            {
                totalPrice = 0m;
                totalQuantity = 0m;
            }

            if (ri.InvoiceType == PricingRelatedInvoiceType.Purchase)
            {
                totalQuantity += ri.InvoiceItemQuantity;
                totalPrice += ri.InvoiceItemPrice;
                average = totalQuantity <= 0 ? 0 : totalPrice / totalQuantity;
            }
            else
            {
                totalQuantity -= ri.InvoiceItemPrice;
                totalPrice -= average * ri.InvoiceItemQuantity;

                if (totalQuantity <= 0)
                {
                    totalQuantity = 0;
                    totalPrice = 0;
                }
            }

            if (ri.InvoiceDate >= startDate)
                averagePrices.Add(new InvoiceAveragePurchasePrice
                {
                    PurchaseInvoiceItemID = ri.InvoiceType == PricingRelatedInvoiceType.Purchase ? ri.InvoiceItemID : null,
                    SaleInvoiceItemID = ri.InvoiceType == PricingRelatedInvoiceType.Sale ? ri.InvoiceItemID : null,
                    AveragePurchasePrice = Math.Round(average)
                });
        }
    }

    public async Task<DateTime> GetNewPricingStartDate()
    {
        return (await _repository.Set.OrderBy(p => p.EndDate).FirstOrDefaultAsync())?.EndDate.Date.AddDays(1)
            ?? (await _generalService.GetStoreInfo()).InitializationDate;
    }

    public Task<bool> IsThereAnyPricingInDate(DateTime date)
    {
        return _repository.Set.AnyAsync(p => p.StartDate <= date && p.EndDate >= date);
    }
}