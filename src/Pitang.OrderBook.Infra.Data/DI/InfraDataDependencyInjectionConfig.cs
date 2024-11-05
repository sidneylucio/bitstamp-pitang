using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Pitang.OrderBook.Domain.Interfaces;
using Pitang.OrderBook.Infra.Data.Repositories;

namespace Pitang.OrderBook.Infra.Data.DI;

public static class InfraDataDependencyInjectionConfig
{
    public static IServiceCollection AddInfraDataServices(this IServiceCollection services)
    {
        services.AddSingleton<IMongoClient>(s =>
            new MongoClient("mongodb://admin:admin@localhost:27017"));

        services.AddSingleton<IOrderBookRepository, OrderBookRepository>();
        services.AddSingleton<ISimulationRepository, SimulationRepository>();

        return services;
    }
}
