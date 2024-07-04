using System.ComponentModel.DataAnnotations;

namespace POS.Core.Inventory;

public sealed class PurchaseInvoice : BaseEntity
{
    public DateTime Date { get; set; }
    public int Number { get; set; }
    [DataType("DECIMAL(19, 4)")] public decimal TotalPrice { get; set; }
    public Vendor Vendor { get; set; } = null!;
    [MaxLength(450)] public string CreatorID { get; set; } = null!;
    public List<PurchaseInvoiceItem> InvoiceItems { get; set; } = [];
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
    public List<CreatePurchaseInvoiceItemRequest> InvoiceItems { get; set; } = [];
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
    public List<UpdatePurchaseInvoiceItemRequest> InvoiceItems { get; set; } = [];
}

public sealed class UpdatePurchaseInvoiceItemRequest
{
    public int? ID { get; set; }
    public int RowNumber { get; set; }
    public int ItemId { get; set; }
    [DataType("DECIMAL(19, 4)")] public decimal Quantity { get; set; }
    [DataType("DECIMAL(19, 4)")] public decimal Price { get; set; }
}