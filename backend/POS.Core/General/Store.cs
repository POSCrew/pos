namespace POS.Core.General;

public sealed class Store : BaseEntity
{
    public string Title { get; set; } = "";
    public string Address { get; set; } = "";
    public DateTime InitializationDate { get; set; }
}