using System.Net;

namespace Pitang.OrderBook.Domain.Exceptions;

public abstract class HttpException : Exception
{
    public HttpStatusCode StatusCode { get; init; }

    public HttpException() : base()
    {
    }

    public HttpException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public HttpException(string? message) : base(message)
    {
    }
}