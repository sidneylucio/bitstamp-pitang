namespace Pitang.OrderBook.Application.Strategies.SimulateBestPrice
{
    public interface IOrderSelectionStrategyFactory
    {
        IOrderSimulationStrategy GetStrategy(string operation);
    }
}