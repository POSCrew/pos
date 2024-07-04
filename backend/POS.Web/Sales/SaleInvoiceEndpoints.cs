using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using POS.Application.Abstractions;
using POS.Core.Sales;

namespace POS.Web.Sales;

public static class SaleInvoiceEndpoints
{
    public static void AddSaleInvoiceEndpoints(this IEndpointRouteBuilder routes)
    {
        var saleInvoices = routes.MapGroup("saleInvoices");
        saleInvoices.MapPost("/", Create);
        saleInvoices.MapPut("/", Update);
        saleInvoices.MapDelete("/", Remove);
        saleInvoices.MapGet("/", GetByID);
        saleInvoices.MapGet("all", GetAll);
        saleInvoices.MapGet("count", GetCount);
    }

    private static Task Remove(
        [FromServices] ISaleInvoiceService saleInvoiceService,
        [FromQuery] int id
    )
    {
        return saleInvoiceService.Remove(id);
    }

    private static async Task<SaleInvoice> Create(
        [FromServices] ISaleInvoiceService saleInvoiceService,
        [FromServices] IHttpContextAccessor httpContextAccessor,
        [FromBody] CreateSaleInvoiceRequest saleInvoice
    )
    {
        var userId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        return await saleInvoiceService.Create(saleInvoice, userId);
    }

    private static async Task<SaleInvoice> Update(
        [FromServices] ISaleInvoiceService saleInvoiceService,
        [FromBody] UpdateSaleInvoiceRequest saleInvoice
    )
    {
        return await saleInvoiceService.Update(saleInvoice);
    }

    private static Task<SaleInvoice?> GetByID(
        [FromServices] ISaleInvoiceService saleInvoiceService,
        [FromQuery] int id
    )
    {
        return saleInvoiceService.GetByID(id);
    }

    private static Task<List<SaleInvoice>> GetAll(
        [FromServices] ISaleInvoiceService saleInvoiceService,
        [FromQuery] int? page,
        [FromQuery] int? pageSize
    )
    {
        return saleInvoiceService.GetAll(page, pageSize);
    }

    private static Task<int> GetCount(
        [FromServices] ISaleInvoiceService saleInvoiceService
    )
    {
        return saleInvoiceService.GetCount();
    }
}
