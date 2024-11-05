using Pitang.OrderBook.Domain.Entities;

namespace Pitang.OrderBook.Domain.Interfaces;

public interface IRepository<T> where T : Entity
{
    Task SaveAsync(T message);
    Task<IEnumerable<T>> GetAllAsync();
}
