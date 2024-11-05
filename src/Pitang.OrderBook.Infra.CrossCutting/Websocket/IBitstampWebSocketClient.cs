using System;
using System.Threading.Tasks;

namespace Pitang.OrderBook.Infra.CrossCutting.Websocket;

public interface IBitstampWebSocketClient
{
    event Action<string> OnMessageReceived;
    Task StartAsync();
    Task StopAsync();
}
