using Microsoft.Extensions.DependencyInjection;
using POS.Application.Abstractions;
using POS.Application.Inventory;
using POS.Application.Sales;

namespace POS.Application;

public static class ApplicationServiceRegistry
{
    public static void RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<IItemService, ItemService>();
        services.AddTransient<IVendorService, VendorService>();
        services.AddTransient<ICustomerService, CustomerService>();
    }
}