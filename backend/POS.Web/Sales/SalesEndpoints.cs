namespace POS.Web.Sales;

public static class SalesEndponits
{
    public static void AddSalesEndpoints(this IEndpointRouteBuilder routes)
    {
        var sales = routes.MapGroup("sales")
            .RequireAuthorization(PosIdentity.SellerOrAdminPolicy);

        CustomerEndpoints.AddCustomerEndpoints(sales);
    }
}
