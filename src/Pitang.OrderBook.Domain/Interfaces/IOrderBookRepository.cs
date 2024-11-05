namespace Pitang.OrderBook.Domain.Interfaces;

public interface IOrderBookRepository : IRepository<Entities.OrderBook>
{
    Task<IEnumerable<Entities.OrderBook>> GetMessagesForLastSecondsAsync(string instrument, int seconds);
    Task<Entities.OrderBook> GetLatestOrderBookAsync(string instrument);
}
