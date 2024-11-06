using Pitang.OrderBook.Domain.Exceptions;

namespace Pitang.OrderBook.Application.Strategies.SimulateBestPrice;
public class OrderSelectionStrategyFactory : IOrderSelectionStrategyFactory
{
    private readonly IEnumerable<IOrderSimulationStrategy> _orderSimulationStrategies;

    public OrderSelectionStrategyFactory(IEnumerable<IOrderSimulationStrategy> orderSimulationStrategies)
    {
        _orderSimulationStrategies = orderSimulationStrategies;
    }
    public IOrderSimulationStrategy GetStrategy(string operation)
    {
        var strategy = _orderSimulationStrategies.FirstOrDefault(x => x.GetSimulationStrategy(operation));

        if (strategy is null)
            throw new BadRequestException("Strategy não encontrada");

        return strategy;
    }
}
