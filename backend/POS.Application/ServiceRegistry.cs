using Microsoft.Extensions.DependencyInjection;
using POS.Application.Abstractions;
using POS.Application.General;
using POS.Application.Inventory;
using POS.Application.Sales;

namespace POS.Application;

public static class ApplicationServiceRegistry
{
    public static void RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<IGeneralService, GeneralService>();
        services.AddTransient<IItemService, ItemService>();
        services.AddTransient<IVendorService, VendorService>();
        services.AddTransient<ICustomerService, CustomerService>();
        services.AddTransient<IPurchaseInvoiceService, PurchaseInvoiceService>();
        services.AddTransient<ISaleInvoiceService, SaleInvoiceService>();
        services.AddTransient<IInventoryReview, InventoryReview>();
        services.AddTransient<IPricingService, PricingService>();
        services.AddTransient<ISalesReview, SalesReview>();
    }
}