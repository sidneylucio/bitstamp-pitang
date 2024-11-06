using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pitang.OrderBook.Application.DI;
using Pitang.OrderBook.Domain.Configurations;
using Pitang.OrderBook.Infra.CrossCutting.DI;
using Pitang.OrderBook.Infra.Data.Extensions;

namespace Pitang.OrderBook.Infra.IoC.DI;

public static class DependencyInjectionConfig
{
    public static IServiceCollection AddOrderBookDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OrderBookSettings>(configuration.GetSection("OrderBook"));
        
        services.AddApplicationDependencies();

        services.AddCrossCuttingServices(configuration);

        services.AddInfraDataServices(configuration);

        return services;
    }
}
