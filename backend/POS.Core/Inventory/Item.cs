using System.ComponentModel.DataAnnotations;

namespace POS.Core.Inventory;

public sealed class Item : BaseEntity
{
    // TODO: set max length for all text fields
    [MaxLength(200)] public string Title { get; set; } = "";
    [MaxLength(100)] public string Serial { get; set; } = "";
    [MaxLength(200)] public string Description { get; set; } = "";
    [DataType("DECIMAL(19, 4)")] public decimal SalePrice { get; set; }
}
