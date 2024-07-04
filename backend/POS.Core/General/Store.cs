using System.ComponentModel.DataAnnotations;

namespace POS.Core.General;

public sealed class Store : BaseEntity
{
    [MaxLength(200)] public string Title { get; set; } = "";
    [MaxLength(200)] public string Address { get; set; } = "";
    public DateTime InitializationDate { get; set; }
}