using MongoDB.Bson.Serialization.Attributes;

namespace NobelSearch.Api.Models;

public class Laureate
{
    [BsonElement("id")]
    public string Id { get; set; } = null!;

    [BsonElement("firstname")]
    public string FirstName { get; set; } = null!;

    [BsonElement("surname")]
    public string? Surname { get; set; }

    [BsonElement("motivation")]
    public string Motivation { get; set; } = null!;

    [BsonElement("share")]
    public string Share { get; set; } = null!;
}