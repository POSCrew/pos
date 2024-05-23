using System.ComponentModel.DataAnnotations;

namespace POS.Core.Sales;

public sealed class Pricing : BaseEntity
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<InvoiceAveragePurchasePrice> InvoiceAveragePurchasePrices { get; set; } = new List<InvoiceAveragePurchasePrice>();
}

public sealed class InvoiceAveragePurchasePrice : BaseEntity
{
    public int? SaleInvoiceItem { get; set; }
    public int? PurchaseInvoiceItem { get; set; }
    [DataType("DECIMAL(19, 4)")] public decimal AveragePurchasePrice { get; set; }
}