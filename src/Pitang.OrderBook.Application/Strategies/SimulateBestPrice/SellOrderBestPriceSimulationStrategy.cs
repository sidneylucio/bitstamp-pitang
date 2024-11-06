using Pitang.OrderBook.Application.DTOs;
using Pitang.OrderBook.Domain.Entities;
using Pitang.OrderBook.Domain.Exceptions;
using Entities = Pitang.OrderBook.Domain.Entities;

namespace Pitang.OrderBook.Application.Strategies.SimulateBestPrice;

public class SellOrderBestPriceSimulationStrategy : IOrderSimulationStrategy
{
    private const string OPERATION = "sell";
    public bool GetSimulationStrategy(string operation)
    {
        return operation == OPERATION;
    }
    public List<OrderDetail> SelectOrders(Entities.OrderBook orderBook, double quantity)
    {
        var orders = orderBook.Bids.OrderByDescending(order => double.Parse(order.Price)).ToList();
        return CalculateOrders(orders, quantity);
    }

    private List<OrderDetail> CalculateOrders(List<OrderBookDetail> orders, double quantity)
    {
        double accumulatedQuantity = 0;
        double totalPrice = 0;
        var usedOrders = new List<OrderDetail>();

        foreach (var order in orders)
        {
            var orderQuantity = double.Parse(order.Quantity);
            var orderPrice = double.Parse(order.Price);

            if (accumulatedQuantity + orderQuantity >= quantity)
            {
                var remainingQuantity = quantity - accumulatedQuantity;
                totalPrice += remainingQuantity * orderPrice;
                usedOrders.Add(new OrderDetail(order.Price, remainingQuantity.ToString()));
                break;
            }
            else
            {
                totalPrice += orderQuantity * orderPrice;
                usedOrders.Add(new OrderDetail(order.Price, order.Quantity));
                accumulatedQuantity += orderQuantity;
            }
        }

        if (false && accumulatedQuantity < quantity)
            throw new BadRequestException($"Quantidade insuficiente para atender a solicitação. Quantidade acumulada: {accumulatedQuantity}");

        return usedOrders;
    }
}
