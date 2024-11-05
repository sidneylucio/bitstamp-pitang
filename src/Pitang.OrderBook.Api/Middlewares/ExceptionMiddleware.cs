using System.Net;
using Pitang.OrderBook.Domain.Exceptions;
using Pitang.OrderBook.Domain.Extensions;

namespace Pitang.OrderBook.Api.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (HttpException ex)
        {
            _logger.LogWarning(ex, ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)ex.StatusCode;
            await context.Response.WriteAsync(ex.Serialize());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(ex.Serialize());
        }
    }
}