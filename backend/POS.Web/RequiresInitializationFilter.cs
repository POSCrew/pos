
using POS.Application.Abstractions;

namespace POS.Web;

public sealed class RequiresInitializationFilter : IEndpointFilter
{
    public static RequiresInitializationFilter Instance = new RequiresInitializationFilter();

    public ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if(!IGeneralService.IsStoreInitialized)
            throw new POSException("store should be initialized");

        return next.Invoke(context);
    }
}