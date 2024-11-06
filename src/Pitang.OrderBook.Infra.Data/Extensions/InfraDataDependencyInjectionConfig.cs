using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Pitang.OrderBook.Domain.Interfaces;
using Pitang.OrderBook.Infra.Data.Repositories;

namespace Pitang.OrderBook.Infra.Data.Extensions;

public static class InfraDataDependencyInjectionConfig
{
    public static IServiceCollection AddInfraDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("Default")!;

        services.AddSingleton<IMongoClient>(s => new MongoClient(connectionString));

        services.AddSingleton<IOrderBookRepository, OrderBookRepository>();
        services.AddSingleton<ISimulationRepository, SimulationRepository>();

        return services;
    }
}
