using MediatR;
using Microsoft.Extensions.Logging;
using Pitang.OrderBook.Application.Commands;
using Pitang.OrderBook.Domain.Interfaces;

namespace Pitang.OrderBook.Application.Handlers;

public class MetricsHandler : IRequestHandler<MetricsCommand, Unit>
{
    private readonly IOrderBookRepository _orderBookRepository;
    private readonly ILogger<MetricsHandler> _logger;

    public MetricsHandler(IOrderBookRepository orderBookRepository, ILogger<MetricsHandler> logger)
    {
        _orderBookRepository = orderBookRepository;
        _logger = logger;
    }

    public async Task<Unit> Handle(MetricsCommand request, CancellationToken cancellationToken)
    {
        var messages = await _orderBookRepository.GetMessagesForLastSecondsAsync(request.Instrument, 5);

        if (!messages.Any())
        {
            _logger.LogInformation($"Nenhum dado disponível para {request.Instrument} nos últimos 5 segundos.");
            return Unit.Value;
        }

        var highestBid = messages.Max(m => m.Bids.Max(b => double.Parse(b.Price)));
        var lowestAsk = messages.Min(m => m.Asks.Min(a => double.Parse(a.Price)));
        var currentAvgPrice = messages.Average(m => m.Bids.Concat(m.Asks).Average(o => double.Parse(o.Price)));
        var avgQuantity = messages.Average(m => m.Bids.Concat(m.Asks).Average(o => double.Parse(o.Quantity)));

        _logger.LogInformation($"[{request.Instrument}]{Environment.NewLine} Maior preço de compra: {highestBid},{Environment.NewLine} Menor preço de venda: {lowestAsk},{Environment.NewLine} Média atual de preço: {currentAvgPrice},{Environment.NewLine} Média de quantidade: {avgQuantity}");

        return Unit.Value;
    }
}
