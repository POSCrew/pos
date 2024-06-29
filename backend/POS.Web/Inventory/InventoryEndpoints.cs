namespace POS.Web.Inventory;

public static class InventoryEndpoints
{
    public static void AddInventoryEndpoints(this IEndpointRouteBuilder routes)
    {
        var inventory = routes.MapGroup("inventory")
            .RequireAuthorization(PosIdentity.SellerOrAdminPolicy);

        ItemEndpoints.AddItemEndpoints(inventory);
        VendorEndpoints.AddVendorEndpoints(inventory);
    }
}
