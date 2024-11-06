namespace Pitang.OrderBook.Infra.CrossCutting.Configurations;

public class BitstampSettings
{
    public string WebSocketURL { get; set; } = string.Empty;
    public string BtcUsdChannel { get; set; } = string.Empty;
    public string EthUsdChannel { get; set; } = string.Empty;
    public string Event { get; set; } = string.Empty;
}
