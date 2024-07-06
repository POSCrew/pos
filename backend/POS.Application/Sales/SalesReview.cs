using POS.Application.Abstractions;
using POS.Application.Abstractions.Data;
using POS.Core.Sales;

namespace POS.Application.Sales;

public sealed class SalesReview : ISalesReview
{
    private readonly IRepository<SaleInvoice> _repository;

    public SalesReview(IRepository<SaleInvoice> repository)
    {
        _repository = repository;
    }

    public async Task<List<SalesReviewProfit>> GetProfitSheetData(SalesReviewFilter filter)
    {
        filter.StartDate = filter.StartDate.Date;
        filter.EndDate = filter.EndDate.Date;
        
        if(filter.EndDate < filter.StartDate)
            throw new POSException("end date should be after start date in sales review");

        return await _repository.ExecuteRawSql<SalesReviewProfit>($"""
DECLARE @StartDate DATE = {filter.StartDate};
DECLARE @EndDate DATE = {filter.EndDate};

WITH InvoiceItems AS (
    SELECT
        PI.ID AS InvoiceItemID,
        'Purchase Invoice' AS InvoiceType,
        P.Number as InvoiceNumber,
        CAST(P.[Date] AS Date) AS InvoiceDay,
        P.[Date] AS InvoiceDate,
        P.Description AS InvoiceDescription,
        PI.ItemID,
        IT.Serial AS ItemSerial,
        IT.Title AS ItemTitle,
        PI.Quantity AS InvoiceItemQuantity,
        PI.Fee AS InvoiceItemFee,
        PI.Price AS InvoiceItemPrice,
        AV.AveragePurchasePrice AS AveragePurchaseFee,
        0 AS Profit -- -PI.Price AS Profit
    FROM PurchaseInvoiceItem PI
    INNER JOIN PurchaseInvoices P ON PI.PurchaseInvoiceID = P.ID
    INNER JOIN Items IT ON PI.ItemID = IT.ID
    INNER JOIN Vendors V ON P.VendorID = V.ID
    LEFT JOIN InvoiceAveragePurchasePrice AV ON PI.ID = AV.PurchaseInvoiceItemID

    UNION ALL

    SELECT
        SI.ID AS InvoiceItemID,
        'Sale Invoice' AS InvoiceType,
        S.Number as InvoiceNumber,
        CAST(S.[Date] AS Date) AS InvoiceDay,
        S.[Date] AS InvoiceDate,
        S.Description AS InvoiceDescription,
        SI.ItemID,
        IT.Serial AS ItemSerial,
        IT.Title AS ItemTitle,
        SI.Quantity AS InvoiceItemQuantity,
        SI.Fee AS InvoiceItemFee,
        SI.Price AS InvoiceItemPrice,
        AV.AveragePurchasePrice AS AveragePurchaseFee,
        SI.Price - (AV.AveragePurchasePrice * SI.Quantity) AS Profit
    FROM SaleInvoiceItem SI
    INNER JOIN SaleInvoices S ON SI.SaleInvoiceID = S.ID
    INNER JOIN Items IT ON SI.ItemID = IT.ID
    INNER JOIN Customers V ON S.CustomerID = V.ID
    LEFT JOIN InvoiceAveragePurchasePrice AV ON SI.ID = AV.SaleInvoiceItemID
)
SELECT *
FROM InvoiceItems
WHERE (@StartDate IS NULL OR InvoiceDay >= @StartDate) AND (@EndDate IS NULL OR InvoiceDay <= @EndDate)
ORDER BY InvoiceItems.ItemID ASC, InvoiceItems.InvoiceDay ASC, InvoiceItems.InvoiceType ASC, InvoiceItems.InvoiceNumber ASC
""");
    }
}

public sealed class SalesReviewProfit
{
    public int InvoiceItemID { get; set; }
    public string? InvoiceType { get; set; }
    public int InvoiceNumber { get; set; }
    public DateTime InvoiceDay { get; set; }
    public DateTime InvoiceDate { get; set; }
    public string? InvoiceDescription { get; set; }
    public int ItemID { get; set; }
    public string? ItemSerial { get; set; }
    public string? ItemTitle { get; set; }
    public decimal InvoiceItemQuantity { get; set; }
    public decimal InvoiceItemFee { get; set; }
    public decimal InvoiceItemPrice { get; set; }
    public decimal? AveragePurchaseFee { get; set; }
    public decimal? Profit { get; set; }
}

public sealed class SalesReviewFilter
{
    public DateTime EndDate { get; set; }
    public DateTime StartDate { get; set; }
}