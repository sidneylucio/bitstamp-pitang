using Microsoft.Extensions.DependencyInjection;
using Pitang.OrderBook.Application.Handlers;
using Pitang.OrderBook.Infra.CrossCutting.DI;
using Pitang.OrderBook.Infra.Data.DI;

namespace Pitang.OrderBook.Infra.IoC.DependencyInjection;

public static class DependencyInjectionConfig
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(LiveOrderBookCommandHandler).Assembly);
        });

        services.AddCrossCuttingServices();

        services.AddInfraDataServices();

        return services;
    }
}
