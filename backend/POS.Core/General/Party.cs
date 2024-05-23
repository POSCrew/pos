namespace POS.Core.General;

public abstract class Party : BaseEntity
{
    public string FirstName { get; set; } = "";
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
}