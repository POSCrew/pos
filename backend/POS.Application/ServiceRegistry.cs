using Microsoft.Extensions.DependencyInjection;
using POS.Application.Abstractions;
using POS.Application.Inventory;

namespace POS.Application;

public static class ApplicationServiceRegistry
{
    public static void RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<IItemService, ItemService>();
        services.AddTransient<IVendorService, VendorService>();
    }
}