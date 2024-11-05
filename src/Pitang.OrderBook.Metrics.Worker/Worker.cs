using MediatR;
using Pitang.OrderBook.Application.Commands;

namespace Pitang.OrderBook.Metrics.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IMediator _mediator;


        public Worker(ILogger<Worker> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }

                await _mediator.Send(new MetricsCommand("order_book_btcusd"), stoppingToken);
                await _mediator.Send(new MetricsCommand("order_book_ethusd"), stoppingToken);

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                Console.Clear();
            }
        }
    }
}
