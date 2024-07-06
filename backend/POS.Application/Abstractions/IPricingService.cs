using POS.Core.Sales;

namespace POS.Application.Abstractions;

public interface IPricingService
{
    Task DoPricing(DateTime startDate, DateTime endDate);
    Task<DateTime> GetNewPricingStartDate();
    Task<bool> IsThereAnyPricingInDate(DateTime date);
    Task<List<Pricing>> GetAll(int? page, int? pageSize);
    Task<int> GetCount();
    Task RemoveLastPricing();
}