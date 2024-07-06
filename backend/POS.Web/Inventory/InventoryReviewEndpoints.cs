using Microsoft.AspNetCore.Mvc;
using POS.Application.Abstractions;
using POS.Application.Inventory;

namespace POS.Web.Inventory;

public static class InventoryReviewEndpoints
{
    public static void AddInventoryReviewEndpoints(this IEndpointRouteBuilder routes)
    {
        var inventoryReview = routes.MapGroup("review")
            .RequireAuthorization(PosIdentity.AdminPolicy);
        
        inventoryReview.MapPost("items", ItemsSheet);
    }

    private static async Task<List<InventoryReviewItems>> ItemsSheet(
        [FromServices] IInventoryReview inventoryReview,
        [FromBody] InventoryReviewFilter filter
    )
    {
        return await inventoryReview.GetItemSheetData(filter);
    }
}
