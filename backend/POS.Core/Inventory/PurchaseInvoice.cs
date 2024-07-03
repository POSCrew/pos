using System.ComponentModel.DataAnnotations;

namespace POS.Core.Inventory;

public sealed class PurchaseInvoice : BaseEntity
{
    public DateTime Date { get; set; }
    public int Number { get; set; }
    [DataType("DECIMAL(19, 4)")] public decimal TotalPrice { get; set; }
    public Vendor Vendor { get; set; } = null!;
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

public sealed class CreatePurchaseInvoiceRequest
{
    public DateTime Date { get; set; }
    public int? Number { get; set; }
    public int? VendorId { get; set; }
    public List<CreatePurchaseInvoiceItemRequest> InvoiceItems { get; set; } = new List<CreatePurchaseInvoiceItemRequest>();
}

public sealed class CreatePurchaseInvoiceItemRequest
{
    public int RowNumber { get; set; }
    public int ItemId { get; set; }
    [DataType("DECIMAL(19, 4)")] public decimal Quantity { get; set; }
    [DataType("DECIMAL(19, 4)")] public decimal Price { get; set; }
}

public sealed class UpdatePurchaseInvoiceRequest
{
    public int ID { get; set; }
    public DateTime Date { get; set; }
    public int? Number { get; set; }
    public int? VendorId { get; set; }
    public List<UpdatePurchaseInvoiceItemRequest> InvoiceItems { get; set; } = new List<UpdatePurchaseInvoiceItemRequest>();
}

public sealed class UpdatePurchaseInvoiceItemRequest
{
    public int? ID { get; set; }
    public int RowNumber { get; set; }
    public int ItemId { get; set; }
    [DataType("DECIMAL(19, 4)")] public decimal Quantity { get; set; }
    [DataType("DECIMAL(19, 4)")] public decimal Price { get; set; }
}