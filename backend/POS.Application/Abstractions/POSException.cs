using System.Net;

namespace POS.Application.Abstractions;

public sealed class POSException : Exception
{
    public HttpStatusCode StatusCode { get; init; }
    public object? MetaData { get; init; }

    public POSException(string message, HttpStatusCode? statusCode = null, object? metaData = null) : base(message)
    {
        StatusCode = statusCode ?? HttpStatusCode.BadRequest;
        MetaData = metaData;
    }
}