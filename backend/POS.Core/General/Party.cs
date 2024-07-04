using System.ComponentModel.DataAnnotations;

namespace POS.Core.General;

public abstract class Party : BaseEntity
{
    [MaxLength(200)] public string Code { get; set; } = "";
    [MaxLength(200)] public string FirstName { get; set; } = "";
    [MaxLength(200)] public string? LastName { get; set; }
    [MaxLength(100)] public string? PhoneNumber { get; set; }
    [MaxLength(300)] public string? Address { get; set; }
}