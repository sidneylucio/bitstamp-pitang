using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Pitang.OrderBook.Domain.Configurations;
using Pitang.OrderBook.Domain.Entities;
using Pitang.OrderBook.Domain.Interfaces;

namespace Pitang.OrderBook.Infra.Data.Repositories;

public class SimulationRepository : Repository<Simulation>, ISimulationRepository
{
    public SimulationRepository(IMongoClient mongoClient, IOptions<OrderBookSettings> options) : base(mongoClient, "Simulation", options)
    {
    }
}
