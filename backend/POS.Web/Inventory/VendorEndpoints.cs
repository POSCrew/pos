using Microsoft.AspNetCore.Mvc;
using POS.Application.Abstractions;
using POS.Core.Inventory;

namespace POS.Web.Inventory;

public static class VendorEndpoints
{
    public static void AddVendorEndpoints(this IEndpointRouteBuilder routes)
    {
        var vendors = routes.MapGroup("vendors")
            .AddEndpointFilter(RequiresInitializationFilter.Instance);
        vendors.MapPost("/", Create);
        vendors.MapPut("/", Update);
        vendors.MapDelete("/", Remove);
        vendors.MapGet("/", GetByID);
        vendors.MapGet("all", GetAll);
        vendors.MapGet("count", GetCount);
    }

    private static Task Remove(
        [FromServices] IVendorService vendorService,
        [FromQuery] int id
    )
    {
        return vendorService.Remove(id);
    }

    private static async Task<Vendor> Create(
        [FromServices] IVendorService vendorService,
        [FromBody] Vendor vendor
    )
    {
        await vendorService.Create(vendor);
        return vendor;
    }

    private static async Task<Vendor> Update(
        [FromServices] IVendorService vendorService,
        [FromBody] Vendor vendor
    )
    {
        await vendorService.Update(vendor);
        return vendor;
    }

    private static Task<Vendor?> GetByID(
        [FromServices] IVendorService vendorService,
        [FromQuery] int id
    )
    {
        return vendorService.GetByID(id);
    }

    private static Task<List<Vendor>> GetAll(
        [FromServices] IVendorService vendorService,
        [FromQuery] int? page,
        [FromQuery] int? pageSize
    )
    {
        return vendorService.GetAll(page, pageSize);
    }

    private static Task<int> GetCount(
        [FromServices] IVendorService vendorService
    )
    {
        return vendorService.GetCount();
    }
}
