using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NobelSearch.Api.Models;

public class Prize
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    [BsonElement("year")]
    public string Year { get; set; } = null!;

    [BsonElement("category")]
    public string Category { get; set; } = null!;

    [BsonElement("laureates")]
    public List<Laureate>? Laureates { get; set; }
}