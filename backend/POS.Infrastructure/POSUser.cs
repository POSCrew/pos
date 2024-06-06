using Microsoft.AspNetCore.Identity;

public sealed class POSUser : IdentityUser
{
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
}