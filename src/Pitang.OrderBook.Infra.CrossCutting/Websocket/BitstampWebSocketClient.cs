using Microsoft.Extensions.Logging;
using System.Net.WebSockets;
using System.Text.Json;
using Websocket.Client;

namespace Pitang.OrderBook.Infra.CrossCutting.Websocket;

public class BitstampWebSocketClient : IBitstampWebSocketClient
{
    private readonly ILogger<BitstampWebSocketClient> _logger;
    private WebsocketClient _client;

    public event Action<string> OnMessageReceived;

    public BitstampWebSocketClient(ILogger<BitstampWebSocketClient> logger)
    {
        _logger = logger;
    }

    public async Task StartAsync()
    {
        var url = new Uri("wss://ws.bitstamp.net");
        _client = new WebsocketClient(url);
        _client.MessageReceived.Subscribe(msg => OnMessageReceived?.Invoke(msg.Text));

        _logger.LogInformation("Conectando ao WebSocket da Bitstamp...");
        await _client.Start();

        SubscribeToChannel("order_book_btcusd");
        SubscribeToChannel("order_book_ethusd");
    }

    private void SubscribeToChannel(string channel)
    {
        var message = new
        {
            @event = "bts:subscribe",
            data = new { channel }
        };

        string jsonMessage = JsonSerializer.Serialize(message);
        _client.Send(jsonMessage);
        _logger.LogInformation($"Subscrito ao canal {channel}");
    }

    public async Task StopAsync()
    {
        _logger.LogInformation("Desconectando do WebSocket...");
        await _client.Stop(WebSocketCloseStatus.NormalClosure, "Desconexão solicitada");
        _client.Dispose();
    }
}
