using MediatR;
using Pitang.OrderBook.Application.Commands;
using Pitang.OrderBook.Application.DTOs;
using Pitang.OrderBook.Application.Strategies.SimulateBestPrice;
using Pitang.OrderBook.Domain.Entities;
using Pitang.OrderBook.Domain.Exceptions;
using Pitang.OrderBook.Domain.Interfaces;

namespace Pitang.OrderBook.Application.Handlers;
public class SimulateBestPriceHandler : IRequestHandler<SimulateBestPriceCommand, SimulationResponseDto>
{
    private readonly ISimulationRepository _simulationRepository;
    private readonly IOrderBookRepository _orderBookRepository;
    private readonly IOrderSelectionStrategyFactory _strategyFactory;

    public SimulateBestPriceHandler(ISimulationRepository simulationRepository, IOrderBookRepository orderBookRepository, IOrderSelectionStrategyFactory strategyFactory)
    {
        _simulationRepository = simulationRepository;
        _orderBookRepository = orderBookRepository;
        _strategyFactory = strategyFactory;
    }

    public async Task<SimulationResponseDto> Handle(SimulateBestPriceCommand request, CancellationToken cancellationToken)
    {
        var latestOrderBook = await _orderBookRepository.GetLatestOrderBookAsync(request.Instrument);
        if (latestOrderBook == null)
            throw new BadRequestException("Nenhum dado de ordem encontrado para o instrumento especificado.");

        var strategy = _strategyFactory.GetStrategy(request.Operation);
        var usedOrders = strategy.SelectOrders(latestOrderBook, request.Quantity);

        var totalPrice = usedOrders.Sum(order => double.Parse(order.Quantity) * double.Parse(order.Price));

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

        await _simulationRepository.SaveAsync(_simulation);
    }
}
