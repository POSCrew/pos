using POS.Application.Abstractions;

namespace POS.Web;

public sealed class RequiresInitializationFilter : IEndpointFilter
{
    private readonly IGeneralService _generalService;

    public RequiresInitializationFilter(IGeneralService generalService)
    {
        _generalService = generalService;
    }

    public ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if(!_generalService.IsStoreInitialized)
            throw new POSException("store should be initialized");

        return next.Invoke(context);
    }
}