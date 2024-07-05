using Microsoft.AspNetCore.Mvc;
using POS.Application.Abstractions;
using POS.Application.Sales;

namespace POS.Web.Sales;

public static class SalesReviewEndpoints
{
    public static void AddSalesReviewEndpoints(this IEndpointRouteBuilder routes)
    {
        var salesReview = routes.MapGroup("reivew")
            .RequireAuthorization(PosIdentity.AdminPolicy);
        
        salesReview.MapPost("profit", ItemsSheet);
    }

    private static async Task<List<SalesReviewProfit>> ItemsSheet(
        [FromServices] ISalesReview salesReview,
        [FromBody] SalesReviewFilter filter
    )
    {
        return await salesReview.GetProfitSheetData(filter);
    }
}
