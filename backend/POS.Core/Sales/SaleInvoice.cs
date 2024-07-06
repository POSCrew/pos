using System.ComponentModel.DataAnnotations;
using POS.Core.Inventory;

namespace POS.Core.Sales;

public sealed class SaleInvoice : BaseEntity
{
    public DateTime Date { get; set; }
    public int Number { get; set; }
    [MaxLength(300)] public string Description { get; set; } = "";
    [DataType("DECIMAL(19, 4)")] public decimal TotalPrice { get; set; }
    public Customer Customer { get; set; } = null!;
    [MaxLength(450)] public string CreatorID { get; set; } = null!;
    public List<SaleInvoiceItem> InvoiceItems { get; set; } = [];
    // TODO: add concurrency stamp
}

public sealed class SaleInvoiceItem : BaseEntity
{
    public int RowNumber { get; set; }
    public Item Item { get; set; } = null!;
    [DataType("DECIMAL(19, 4)")] public decimal Quantity { get; set; }
    [DataType("DECIMAL(19, 4)")] public decimal Fee { get; set; }
    [DataType("DECIMAL(19, 4)")] public decimal Price { get; set; }
}

public sealed class CreateSaleInvoiceRequest
{
    public DateTime Date { get; set; }
    public int? Number { get; set; }
    public string Description { get; set; } = "";
    public int? CustomerId { get; set; }
    public List<CreateSaleInvoiceItemRequest> InvoiceItems { get; set; } = [];
}

public sealed class CreateSaleInvoiceItemRequest
{
    public int RowNumber { get; set; }
    public int ItemId { get; set; }
    [DataType("DECIMAL(19, 4)")] public decimal Quantity { get; set; }
    [DataType("DECIMAL(19, 4)")] public decimal Price { get; set; }
}

public sealed class UpdateSaleInvoiceRequest
{
    public int ID { get; set; }
    public DateTime Date { get; set; }
    public int? Number { get; set; }
    public string Description { get; set; } = "";
    public int? CustomerId { get; set; }
    public List<UpdateSaleInvoiceItemRequest> InvoiceItems { get; set; } = [];
}

public sealed class UpdateSaleInvoiceItemRequest
{
    public int? ID { get; set; }
    public int RowNumber { get; set; }
    public int ItemId { get; set; }
    [DataType("DECIMAL(19, 4)")] public decimal Quantity { get; set; }
    [DataType("DECIMAL(19, 4)")] public decimal Price { get; set; }
}