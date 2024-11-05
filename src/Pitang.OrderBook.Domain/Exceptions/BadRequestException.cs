using System.Net;

namespace Pitang.OrderBook.Domain.Exceptions;

public sealed class BadRequestException : HttpException
{
    public BadRequestException(string message) : base(message)
    {
        StatusCode = HttpStatusCode.BadRequest;
    }
}