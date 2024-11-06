using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pitang.OrderBook.Infra.CrossCutting.Configurations;
using System.Net.WebSockets;
using System.Text.Json;
using Websocket.Client;

namespace Pitang.OrderBook.Infra.CrossCutting.Websocket;

public class BitstampWebSocketClient : IBitstampWebSocketClient
{
    private readonly ILogger<BitstampWebSocketClient> _logger;
    private WebsocketClient _client;
    private readonly BitstampSettings _configuration;

    public event Action<string> OnMessageReceived;

    public BitstampWebSocketClient(ILogger<BitstampWebSocketClient> logger, IOptions<BitstampSettings> options)
    {
        _logger = logger;
        _configuration = options.Value;
    }

    public async Task StartAsync()
    {
        var url = new Uri(_configuration.WebSocketURL);
        _client = new WebsocketClient(url);
        _client.MessageReceived.Subscribe(msg => OnMessageReceived?.Invoke(msg.Text!));

        _logger.LogInformation("Conectando ao WebSocket da Bitstamp...");
        await _client.Start();

        SubscribeToChannel(_configuration.BtcUsdChannel);
        SubscribeToChannel(_configuration.EthUsdChannel);
    }

    private void SubscribeToChannel(string channel)
    {
        var message = new
        {
            @event = _configuration.Event,
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
