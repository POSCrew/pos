using System.ComponentModel.DataAnnotations;

namespace POS.Core.Inventory;

public sealed class PurchaseInvoice : BaseEntity
{
    public DateTime Date { get; set; }
    public int Number { get; set; }
    [DataType("DECIMAL(19, 4)")] public decimal TotalPrice { get; set; }
    public Vendor Vendor { get; set; } = Vendor.DefaultVendor;
    public int CreatorID { get; set; }
    public List<PurchaseInvoiceItem> InvoiceItems { get; set; } = new List<PurchaseInvoiceItem>();
    // TODO: add concurrency stamp
}

public sealed class PurchaseInvoiceItem : BaseEntity
{
    public int RowNumber { get; set; }
    public Item Item { get; set; } = null!;
    [DataType("DECIMAL(19, 4)")] public decimal Quantity { get; set; }
    [DataType("DECIMAL(19, 4)")] public decimal Fee { get; set; }
    [DataType("DECIMAL(19, 4)")] public decimal Price { get; set; }
}