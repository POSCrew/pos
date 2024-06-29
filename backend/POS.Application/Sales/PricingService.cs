using Microsoft.EntityFrameworkCore;
using POS.Application.Abstractions;
using POS.Application.Abstractions.Data;
using POS.Core.Sales;

namespace POS.Application.Sales;

public sealed class PricingService : IPricingService
{
    private readonly IRepository<Pricing> _repository;

    public PricingService(IRepository<Pricing> repository)
    {
        _repository = repository;
    }

    public Task<bool> IsThereAnyPricingInDate(DateTime date)
    {
        return _repository.Set.AnyAsync(p => p.StartDate <= date && p.EndDate >= date);
    }
}