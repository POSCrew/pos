using Microsoft.AspNetCore.Mvc;
using POS.Application.Abstractions;
using POS.Core.Sales;

namespace POS.Web.Sales;

public static class PricingEndpoints
{
    public static void AddPricingEndpoints(this IEndpointRouteBuilder routes)
    {
        var pricings = routes.MapGroup("pricings")
            .RequireAuthorization(PosIdentity.AdminPolicy);

        pricings.MapPost("/", Create);
        pricings.MapGet("newPricingStartDate", NewPricingStartDate);
        pricings.MapDelete("/", Remove);
        pricings.MapGet("all", GetAll);
        pricings.MapGet("count", GetCount);
    }

    private static async Task<DateTime> NewPricingStartDate(
        [FromServices] IPricingService pricingService
    )
    {
        return await pricingService.GetNewPricingStartDate();
    }

    private static Task Remove(
        [FromServices] IPricingService pricingService,
        [FromQuery] int id
    )
    {
        return pricingService.Remove(id);
    }

    public sealed record DoPricingRequest(DateTime StartDate, DateTime EndDate);
    private static async Task Create(
        [FromServices] IPricingService pricingService,
        [FromBody] DoPricingRequest pricing
    )
    {
        await pricingService.DoPricing(pricing.StartDate, pricing.EndDate);
    }

    private static Task<List<Pricing>> GetAll(
        [FromServices] IPricingService pricingService,
        [FromQuery] int? page,
        [FromQuery] int? pageSize
    )
    {
        return pricingService.GetAll(page, pageSize);
    }

    private static Task<int> GetCount(
        [FromServices] IPricingService pricingService
    )
    {
        return pricingService.GetCount();
    }
}
