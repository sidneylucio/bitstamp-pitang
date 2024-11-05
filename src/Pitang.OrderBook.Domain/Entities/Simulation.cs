using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Pitang.OrderBook.Domain.Entities;

public class Simulation : Entity
{
    [BsonElement("operation")]
    public string Operation { get; set; } = string.Empty;

    [BsonElement("instrument")]
    public string Instrument { get; set; } = string.Empty;

    [BsonElement("quantity_requested")]
    public double Quantity { get; set; }

    [BsonElement("total_price")]
    public double TotalPrice { get; set; }

    [BsonElement("used_orders")]
    public List<OrderBookDetail> UsedOrders { get; set; } = [];
}
