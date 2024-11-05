using System.Text.Json;

namespace Pitang.OrderBook.Domain.Extensions;

public static class ExceptionExtension
{
    public static string Serialize(this Exception exception)
    {
        if(exception.InnerException == null)
        {
            return JsonSerializer.Serialize(new { exception.Message });
        }

        return JsonSerializer.Serialize(new { exception.Message, Exception = exception.InnerException });
    }
}