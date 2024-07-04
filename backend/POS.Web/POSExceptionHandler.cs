using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POS.Application.Abstractions;

public sealed class POSExceptionHandler : IExceptionHandler
{
    private readonly ILogger<POSExceptionHandler> _logger;

    public POSExceptionHandler(ILogger<POSExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is POSException pexp)
        {
            _logger.LogWarning(pexp, "POSException");
            httpContext.Response.ContentType = "application/problem+json";
            httpContext.Response.StatusCode = (int)pexp.StatusCode;
            var problem = new ProblemDetails
            {
                Title = pexp.Message,
                Status = (int)pexp.StatusCode,
            };

            if (pexp.MetaData != null)
            {
                problem.Extensions = new Dictionary<string, object?>()
                {
                    {"metaData", pexp.MetaData}
                };
            }

            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(problem), cancellationToken);
            return true;
        }

        if (exception is DbUpdateConcurrencyException dexp)
        {
            _logger.LogWarning(dexp, "DbUpdateConcurrencyException");
            httpContext.Response.ContentType = "application/problem+json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(
                new ProblemDetails
                {
                    Title = "entity does not exist",
                    Status = (int)HttpStatusCode.NotFound
                }
            ), cancellationToken);
            return true;
        }

        return false;
    }
}