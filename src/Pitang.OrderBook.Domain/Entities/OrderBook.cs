using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Pitang.OrderBook.Domain.Entities;

public class OrderBook : Entity
{
    [BsonElement("timestamp")]
    public DateTime Timestamp { get; set; }

    [BsonElement("microtimestamp")]
    public long MicroTimestamp { get; set; }

    [BsonElement("bids")]
    public IList<OrderBookDetail> Bids { get; set; } = [];

    [BsonElement("asks")]
    public IList<OrderBookDetail> Asks { get; set; } = [];

    [BsonElement("channel")]
    public string Channel { get; set; } = string.Empty;

    [BsonElement("event")]
    public string Event { get; set; } = string.Empty;
}
