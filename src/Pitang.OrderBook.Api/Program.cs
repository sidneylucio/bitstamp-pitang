using Pitang.OrderBook.Api.Extensions;
using Pitang.OrderBook.Api.Middlewares;
using Pitang.OrderBook.Infra.IoC.DI;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddSwaggerServices();
        builder.Services.AddControllers();
        builder.Services.AddOrderBookDependencies(builder.Configuration);

        var app = builder.Build();

        if (true || app.Environment.IsDevelopment())
        {
            app.UseCustomSwagger();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.UseMiddleware<ExceptionMiddleware>();
        app.Run();
    }
}
