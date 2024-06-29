using Microsoft.AspNetCore.Mvc;
using POS.Application.Abstractions;
using POS.Core.Inventory;

namespace POS.Web.Inventory;

public static class ItemEndponits
{
    public static void AddItemEndpoints(this IEndpointRouteBuilder routes)
    {
        var items = routes.MapGroup("items");
        items.MapPost("/", Create);
        items.MapPut("/", Update);
        items.MapDelete("/", Remove);
        items.MapGet("/", GetByID);
        items.MapGet("all", GetAll);
        items.MapGet("count", GetCount);
    }

    private static Task Remove(
        [FromServices] IItemService itemService,
        [FromQuery] int id
    )
    {
        return itemService.Remove(id);
    }

    private static async Task<Item> Create(
        [FromServices] IItemService itemService,
        [FromBody] Item item
    )
    {
        await itemService.Create(item);
        return item;
    }

    private static async Task<Item> Update(
        [FromServices] IItemService itemService,
        [FromBody] Item item
    )
    {
        await itemService.Update(item);
        return item;
    }

    private static Task<Item?> GetByID(
        [FromServices] IItemService itemService,
        [FromQuery] int id
    )
    {
        return itemService.GetByID(id);
    }

    private static Task<List<Item>> GetAll(
        [FromServices] IItemService itemService,
        [FromQuery] int? page,
        [FromQuery] int? pageSize
    )
    {
        return itemService.GetAll(page, pageSize);
    }

    private static Task<int> GetCount(
        [FromServices] IItemService itemService
    )
    {
        return itemService.GetCount();
    }
}
