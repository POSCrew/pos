namespace POS.Web.Sales;

public static class SalesEndpoints
{
    public static void AddSalesEndpoints(this IEndpointRouteBuilder routes)
    {
        var sales = routes.MapGroup("sales")
            .RequireAuthorization(PosIdentity.SellerOrAdminPolicy)
            .AddEndpointFilter<RequiresInitializationFilter>();

        CustomerEndpoints.AddCustomerEndpoints(sales);
        SaleInvoiceEndpoints.AddSaleInvoiceEndpoints(sales);
        PricingEndpoints.AddPricingEndpoints(sales);
        SalesReviewEndpoints.AddSalesReviewEndpoints(sales);
    }
}
