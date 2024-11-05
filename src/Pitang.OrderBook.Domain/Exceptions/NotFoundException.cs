using System.Net;

namespace Pitang.OrderBook.Domain.Exceptions;

public sealed class NotFoundException : HttpException
{
    public NotFoundException(string message) : base(message)
    {
        base.StatusCode = HttpStatusCode.NotFound;
    }
}