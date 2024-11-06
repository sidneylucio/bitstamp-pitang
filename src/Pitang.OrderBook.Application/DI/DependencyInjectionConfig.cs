using Microsoft.Extensions.DependencyInjection;
using Pitang.OrderBook.Application.Handlers;
using Pitang.OrderBook.Application.Strategies.SimulateBestPrice;

namespace Pitang.OrderBook.Application.DI;

public static class DependencyInjectionConfig
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
    {
        services.AddSingleton<IOrderSelectionStrategyFactory, OrderSelectionStrategyFactory>();
        services.AddSingleton<IOrderSimulationStrategy, BuyOrderBestPriceSimulationStrategy>();
        services.AddSingleton<IOrderSimulationStrategy, SellOrderBestPriceSimulationStrategy>();

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(LiveOrderBookCommandHandler).Assembly);
        });

        return services;
    }
}
