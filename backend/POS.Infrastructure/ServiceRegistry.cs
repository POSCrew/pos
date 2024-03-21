using Microsoft.Extensions.DependencyInjection;
using POS.Application.Data;

namespace POS.Infrastructure;

public static class InfrastructureServiceRegistry
{
    public static void RegisterInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>),typeof(Repository<>));
    }
}