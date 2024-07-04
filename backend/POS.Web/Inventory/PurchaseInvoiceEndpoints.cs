using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using POS.Application.Abstractions;
using POS.Core.Inventory;

namespace POS.Web.Inventory;

public static class PurchaseInvoiceEndpoints
{
    public static void AddPurchaseInvoiceEndpoints(this IEndpointRouteBuilder routes)
    {
        var purcahseInvoices = routes.MapGroup("purcahseInvoices");
        purcahseInvoices.MapPost("/", Create);
        purcahseInvoices.MapPut("/", Update);
        purcahseInvoices.MapDelete("/", Remove);
        purcahseInvoices.MapGet("/", GetByID);
        purcahseInvoices.MapGet("all", GetAll);
        purcahseInvoices.MapGet("count", GetCount);
    }

    private static Task Remove(
        [FromServices] IPurchaseInvoiceService purcahseInvoiceService,
        [FromQuery] int id
    )
    {
        return purcahseInvoiceService.Remove(id);
    }

    private static async Task<PurchaseInvoice> Create(
        [FromServices] IPurchaseInvoiceService purcahseInvoiceService,
        [FromServices] IHttpContextAccessor httpContextAccessor,
        [FromBody] CreatePurchaseInvoiceRequest purcahseInvoice
    )
    {
        var userId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        return await purcahseInvoiceService.Create(purcahseInvoice, userId);
    }

    private static async Task<PurchaseInvoice> Update(
        [FromServices] IPurchaseInvoiceService purcahseInvoiceService,
        [FromBody] UpdatePurchaseInvoiceRequest purcahseInvoice
    )
    {
        return await purcahseInvoiceService.Update(purcahseInvoice);
    }

    private static Task<PurchaseInvoice?> GetByID(
        [FromServices] IPurchaseInvoiceService purcahseInvoiceService,
        [FromQuery] int id
    )
    {
        return purcahseInvoiceService.GetByID(id);
    }

    private static Task<List<PurchaseInvoice>> GetAll(
        [FromServices] IPurchaseInvoiceService purcahseInvoiceService,
        [FromQuery] int? page,
        [FromQuery] int? pageSize
    )
    {
        return purcahseInvoiceService.GetAll(page, pageSize);
    }

    private static Task<int> GetCount(
        [FromServices] IPurchaseInvoiceService purcahseInvoiceService
    )
    {
        return purcahseInvoiceService.GetCount();
    }
}
