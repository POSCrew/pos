namespace POS.Application.Abstractions;

public interface IPricingService
{
    Task<bool> IsThereAnyPricingInDate(DateTime date);
}