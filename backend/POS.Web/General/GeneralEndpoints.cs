
using Microsoft.AspNetCore.Mvc;
using POS.Application.Abstractions;
using POS.Core.General;

namespace POS.Web.Inventory;

public static class GeneralEndpoints
{
    public static void AddGeneralEndpoints(this IEndpointRouteBuilder routes)
    {
        var general = routes.MapGroup("general");
        general.MapGet("isStoreInitialized", IsStoreInitialized);
        general.MapPost("initialize", Initialize);
        general.MapGet("store", StoreInfo)
            .AddEndpointFilter<RequiresInitializationFilter>();
    }

    private static Task<Store> StoreInfo(
        [FromServices] IGeneralService generalService
    )
    {
        return generalService.GetStoreInfo();
    }

    private static bool IsStoreInitialized(
        [FromServices] IGeneralService generalService
    )
    {
        return generalService.IsStoreInitialized;
    }

    private static async Task<Store> Initialize(
        [FromServices] IGeneralService generalService,
        [FromBody] Store store
    )
    {
        return await generalService.InitializeStore(store);
    }
}
