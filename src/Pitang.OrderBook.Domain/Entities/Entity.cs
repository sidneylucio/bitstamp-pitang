using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Pitang.OrderBook.Domain.Entities;

public class Entity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
