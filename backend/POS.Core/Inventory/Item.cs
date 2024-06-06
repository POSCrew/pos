using System.ComponentModel.DataAnnotations;

namespace POS.Core.Inventory;

public sealed class Item : BaseEntity
{
    // TODO: set max length for all text fields
    public string Title { get; set; } = "";
    public string Serial { get; set; } = "";
    public string Description { get; set; } = "";
    [DataType("DECIMAL(19, 4)")] public decimal SalePrice { get; set; }
}
