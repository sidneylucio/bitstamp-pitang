namespace Pitang.OrderBook.Application.Events;

public record OrderBookMessage(
    OrderBookDetails Data,
    string Channel,
    string Event
);

public record OrderBookDetails(
    string Timestamp,
    string MicroTimestamp,
    List<List<string>> Bids,
    List<List<string>> Asks
);
