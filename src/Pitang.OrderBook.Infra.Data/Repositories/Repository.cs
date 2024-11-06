using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Pitang.OrderBook.Domain.Configurations;
using Pitang.OrderBook.Domain.Entities;
using Pitang.OrderBook.Domain.Interfaces;

namespace Pitang.OrderBook.Infra.Data.Repositories;

public class Repository<T> : IRepository<T> where T : Entity
{
    protected readonly IMongoCollection<T> Collection;

    public Repository(IMongoClient mongoClient, string collectionName, IOptions<OrderBookSettings> options)
    {
        var database = mongoClient.GetDatabase(options.Value.DatabaseName);
        Collection = database.GetCollection<T>(collectionName);
    }

    public async Task SaveAsync(T message)
    {
        await Collection.InsertOneAsync(message);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await Collection.Find(_ => true).ToListAsync();
    }
}
