using System.ComponentModel.DataAnnotations;
using POS.Core.Inventory;

namespace POS.Core.Sales;

public sealed class SaleInvoice : BaseEntity
{
    public DateTime Date { get; set; }
    public int Number { get; set; }
    [DataType("DECIMAL(19, 4)")] public decimal TotalPrice { get; set; }
    public Customer Customer { get; set; } = Customer.DefaultCustomer;
    public int CreatorID { get; set; }
    public List<SaleInvoiceItem> InvoiceItems { get; set; } = new List<SaleInvoiceItem>();
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