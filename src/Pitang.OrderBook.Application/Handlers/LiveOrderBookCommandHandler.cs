using MediatR;
using Pitang.OrderBook.Application.Commands;
using Pitang.OrderBook.Application.Events;
using Pitang.OrderBook.Domain.Entities;
using Pitang.OrderBook.Domain.Interfaces;
using Pitang.OrderBook.Infra.CrossCutting.Websocket;
using System.Text.Json;
using System.Threading.Channels;

namespace Pitang.OrderBook.Application.Handlers
{
    public class LiveOrderBookCommandHandler : IRequestHandler<LiveOrderBookCommand>
    {
        private readonly IBitstampWebSocketClient _webSocketClient;
        private readonly IOrderBookRepository _orderBookRepository;
        private readonly Channel<string> _messageChannel;
        private readonly CancellationTokenSource _cts = new();

        public LiveOrderBookCommandHandler(IBitstampWebSocketClient webSocketClient, IOrderBookRepository orderBookRepository)
        {
            _webSocketClient = webSocketClient;
            _orderBookRepository = orderBookRepository;
            _messageChannel = Channel.CreateUnbounded<string>();

            _webSocketClient.OnMessageReceived += async message =>
            {
                await _messageChannel.Writer.WriteAsync(message);
            };

            _ = Task.Run(() => ProcessMessages(_cts.Token));
        }

        public async Task Handle(LiveOrderBookCommand request, CancellationToken cancellationToken)
        {
            await _webSocketClient.StartAsync();
        }

        private async Task ProcessMessages(CancellationToken token)
        {
            await foreach (var message in _messageChannel.Reader.ReadAllAsync(token))
            {
                try
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var messageData = JsonSerializer.Deserialize<OrderBookMessage>(message, options);

                    if (messageData != null && messageData.Event == "data")
                    {
                        var orderBook = new Domain.Entities.OrderBook
                        {
                            Timestamp = DateTimeOffset.FromUnixTimeSeconds(long.Parse(messageData.Data.Timestamp)).UtcDateTime,
                            MicroTimestamp = long.Parse(messageData.Data.MicroTimestamp),
                            Bids = messageData.Data.Bids.Select(bid => new OrderBookDetail(bid[0], bid[1])).ToList(),
                            Asks = messageData.Data.Asks.Select(ask => new OrderBookDetail(ask[0], ask[1])).ToList(),
                            Channel = messageData.Channel,
                            Event = messageData.Event
                        };

                        await _orderBookRepository.SaveAsync(orderBook);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao processar mensagem do WebSocket: {ex.Message}");
                }
            }
        }

        public async Task StopAsync()
        {
            _cts.Cancel();
            await _webSocketClient.StopAsync();
        }
    }
}
