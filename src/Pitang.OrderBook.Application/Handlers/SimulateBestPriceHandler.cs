using MediatR;
using Pitang.OrderBook.Application.Commands;
using Pitang.OrderBook.Application.DTOs;
using Pitang.OrderBook.Domain.Entities;
using Pitang.OrderBook.Domain.Exceptions;
using Pitang.OrderBook.Domain.Interfaces;

namespace Pitang.OrderBook.Application.Handlers;

public class SimulateBestPriceHandler(ISimulationRepository simulationRepository, IOrderBookRepository orderBookRepository) : IRequestHandler<SimulateBestPriceCommand, SimulationResponseDto>
{
    public async Task<SimulationResponseDto> Handle(SimulateBestPriceCommand request, CancellationToken cancellationToken)
    {
        var latestOrderBook = await orderBookRepository.GetLatestOrderBookAsync(request.Instrument);
        if (latestOrderBook == null)
            throw new BadRequestException("Nenhum dado de ordem encontrado para o instrumento especificado.");

        var orders = request.Operation.ToLower() == "buy"
            ? latestOrderBook.Asks.OrderBy(order => double.Parse(order.Price)).ToList()
            : latestOrderBook.Bids.OrderByDescending(order => double.Parse(order.Price)).ToList();

        double accumulatedQuantity = 0;
        double totalPrice = 0;
        var usedOrders = new List<OrderDetail>();

        foreach (var order in orders)
        {
            var orderQuantity = double.Parse(order.Quantity);
            var orderPrice = double.Parse(order.Price);

            if (accumulatedQuantity + orderQuantity >= request.Quantity)
            {
                var remainingQuantity = request.Quantity - accumulatedQuantity;
                totalPrice += remainingQuantity * orderPrice;
                usedOrders.Add(new OrderDetail(order.Price, remainingQuantity.ToString()));
                accumulatedQuantity = request.Quantity;
                break;
            }
            else
            {
                totalPrice += orderQuantity * orderPrice;
                usedOrders.Add(new OrderDetail(order.Price, order.Quantity));
                accumulatedQuantity += orderQuantity;
            }
        }

        if (accumulatedQuantity < request.Quantity)
            throw new BadRequestException($"Quantidade insuficiente para atender a solicitação. Quantidade acumulada: {accumulatedQuantity}");

        var response = new SimulationResponseDto(
            Guid.NewGuid(),
            request.Operation,
            request.Instrument,
            request.Quantity,
            totalPrice,
            usedOrders
        );

        await SaveSimulation(response);

        return response;
    }

    private async Task SaveSimulation(SimulationResponseDto simulation)
    {
        var _simulation = new Simulation
        {
            Operation = simulation.Operation,
            Instrument = simulation.Instrument,
            Quantity = simulation.QuantityRequested,
            TotalPrice = simulation.TotalPrice,
            UsedOrders = simulation.UsedOrders.Select(x => new OrderBookDetail(x.Price, x.Quantity)).ToList()
        };

        await simulationRepository.SaveAsync(_simulation);
    }
}
