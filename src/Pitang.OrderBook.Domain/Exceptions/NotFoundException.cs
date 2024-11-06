using System.Net;

namespace Pitang.OrderBook.Domain.Exceptions;

public sealed class NotFoundException : HttpException
{
    public NotFoundException(string message) : base(message) => StatusCode = HttpStatusCode.NotFound;
}