using Microsoft.Extensions.DependencyInjection;
using POS.Application.Abstractions.Data;

namespace POS.Infrastructure;

public static class InfrastructureServiceRegistry
{
    public static void RegisterInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>),typeof(Repository<>));
        services.AddScoped<ITransactionManager, TransactionManager>();
    }
}