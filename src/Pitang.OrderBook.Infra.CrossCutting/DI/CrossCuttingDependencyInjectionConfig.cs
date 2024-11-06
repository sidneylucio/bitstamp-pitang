using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pitang.OrderBook.Infra.CrossCutting.Configurations;
using Pitang.OrderBook.Infra.CrossCutting.Logging;
using Pitang.OrderBook.Infra.CrossCutting.Websocket;

namespace Pitang.OrderBook.Infra.CrossCutting.DI;

public static class CrossCuttingDependencyInjectionConfig
{
    public static IServiceCollection AddCrossCuttingServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.Configure<BitstampSettings>(configuration.GetSection("Bitstamp"));

        services.AddSingleton<ILoggerManager, LoggerManager>();
        services.AddSingleton<IBitstampWebSocketClient, BitstampWebSocketClient>();

        return services;
    }
}
