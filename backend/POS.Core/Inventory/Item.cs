using System.ComponentModel.DataAnnotations;

namespace POS.Core.Inventory;

public sealed class Item : BaseEntity
{
    public string title { get; set; } = "";
    public string description { get; set; } = "";
    [DataType("DECIMAL(19, 4)")] public decimal SalePrice { get; set; }
}
