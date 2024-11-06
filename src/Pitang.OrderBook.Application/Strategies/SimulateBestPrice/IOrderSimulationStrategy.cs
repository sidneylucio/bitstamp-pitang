using Pitang.OrderBook.Application.DTOs;
using Entities = Pitang.OrderBook.Domain.Entities;

namespace Pitang.OrderBook.Application.Strategies.SimulateBestPrice;

public interface IOrderSimulationStrategy
{
    List<OrderDetail> SelectOrders(Entities.OrderBook orderBook, double quantity);
    bool GetSimulationStrategy(string operation);
}
