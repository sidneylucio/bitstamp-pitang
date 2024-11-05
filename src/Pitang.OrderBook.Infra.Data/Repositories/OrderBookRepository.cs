using MongoDB.Driver;
using Entities = Pitang.OrderBook.Domain.Entities;
using Pitang.OrderBook.Domain.Interfaces;

namespace Pitang.OrderBook.Infra.Data.Repositories;

public class OrderBookRepository : Repository<Entities.OrderBook>, IOrderBookRepository
{
    public OrderBookRepository(IMongoClient mongoClient) : base(mongoClient, "OrderBook")
    {
    }

    public async Task<IEnumerable<Entities.OrderBook>> GetMessagesForLastSecondsAsync(string instrument, int seconds)
    {
        var filterBuilder = Builders<Entities.OrderBook>.Filter;
        var filter = filterBuilder.Eq(m => m.Channel, instrument) &
                     filterBuilder.Gte(m => m.Timestamp, DateTime.UtcNow.AddSeconds(-seconds));

        var result = await Collection.Find(filter).ToListAsync();
        return result;
    }

    public async Task<Entities.OrderBook> GetLatestOrderBookAsync(string instrument)
    {
        var filter = Builders<Entities.OrderBook>.Filter.Eq(ob => ob.Channel, instrument);
        var sort = Builders<Entities.OrderBook>.Sort.Descending(ob => ob.Timestamp);

        var result = await Collection.Find(filter)
                                    .Sort(sort)
                                    .FirstOrDefaultAsync();
        return result;
    }
}
