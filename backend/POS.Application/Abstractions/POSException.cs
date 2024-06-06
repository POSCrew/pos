using System.Net;

namespace POS.Application.Abstractions;

public sealed class POSException : Exception
{
    public HttpStatusCode StatusCode { get; init; }

    public POSException(string message, HttpStatusCode? statusCode = null) : base(message)
    {
        StatusCode = statusCode ?? HttpStatusCode.BadRequest;
    }
}