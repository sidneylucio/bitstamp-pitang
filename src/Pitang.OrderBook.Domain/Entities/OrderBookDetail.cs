using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Pitang.OrderBook.Domain.Entities;

public class OrderBookDetail
{
    [BsonElement("price")]
    public string Price { get; set; }

    [BsonElement("quantity")]
    public string Quantity { get; set; }

    public OrderBookDetail(string price, string quantity)
    {
        Price = price;
        Quantity = quantity;
    }
}
