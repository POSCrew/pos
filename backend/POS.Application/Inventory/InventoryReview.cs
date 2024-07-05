using POS.Application.Abstractions;
using POS.Application.Abstractions.Data;
using POS.Core.Inventory;

namespace POS.Application.Inventory;

public sealed class InventoryReview : IInventoryReview
{
    private readonly IRepository<Item> _repository;

    public InventoryReview(IRepository<Item> repository)
    {
        _repository = repository;
    }

    public Task<List<InventoryReviewItems>> GetItemSheetData(InventoryReviewFilter filter)
    {
        return _repository.ExecuteRawSql<InventoryReviewItems>($"""
DECLARE @ItemID INT = {filter.ItemID};
SELECT
    Invoices.ItemID,
    IT.Serial AS ItemSerial,
    IT.Title AS ItemTitle,
    CASE
        WHEN Invoices.[Type] = 0 THEN 'Purchase Invoice'
        ELSE 'Sale Invoice'
    END AS InvoiceType,
    Invoices.[Date] AS InvoiceDate,
    Invoices.Number AS InvoiceNumber,
    Invoices.InvoiceQuantity,
    Invoices.Price AS InvoicePrice,
    Invoices.Fee AS InvoiceFee,
    SUM(Invoices.InvoiceQuantity) OVER (PARTITION BY Invoices.ItemID ORDER BY Invoices.[Day] ASC, [Type] ASC) AS RunningQuantity
FROM
(
    SELECT CAST(P.[Date] AS Date) AS [Day], P.[Date], P.Number, PI.ItemID, PI.Quantity AS InvoiceQuantity, PI.Price, PI.Fee, 0 AS [Type]
    FROM PurchaseInvoiceItem PI
    INNER JOIN PurchaseInvoices P ON PI.PurchaseInvoiceID = P.ID

    UNION ALL

    SELECT CAST(S.[Date] AS Date) AS [Day], S.[Date], S.Number, SI.ItemID, -SI.Quantity AS InvoiceQuantity, -SI.Price, -SI.Fee, 1 AS [Type]
    FROM SaleInvoiceItem SI
    INNER JOIN SaleInvoices S ON SI.SaleInvoiceID = S.ID
) Invoices
INNER JOIN Items IT ON Invoices.ItemID = IT.ID
WHERE (@ItemID IS NULL OR @ItemID = -1 OR IT.ID = @ItemID)
ORDER BY Invoices.[Day] ASC, [Type] ASC
""");
    }
}

public sealed class InventoryReviewItems
{
    public int ItemID { get; set; }
    public string? ItemSerial { get; set; }
    public string? ItemTitle { get; set; }
    public string? InvoiceType { get; set; }
    public DateTime InvoiceDate { get; set; }
    public int InvoiceNumber { get; set; }
    public decimal InvoiceQuantity { get; set; }
    public decimal InvoicePrice { get; set; }
    public decimal InvoiceFee { get; set; }
    public decimal RunningQuantity { get; set; }
}

public sealed class InventoryReviewFilter
{
    public int? ItemID { get; set; }
}