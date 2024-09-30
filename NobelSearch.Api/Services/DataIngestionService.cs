using System.Text.Json.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json;
using NobelSearch.Api.Models;

namespace NobelSearch.Api.Services;

public class DataIngestionService
{
    private readonly IMongoCollection<Prize> _prizeCollection;

    public DataIngestionService(IDatabaseSettings settings, IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase(settings.DatabaseName);
        _prizeCollection = database.GetCollection<Prize>(settings.NobelPrizeCollectionName);
    }

    public async Task IngestDataAsync()
    {
        // Check if data already exists
        var count = await _prizeCollection.EstimatedDocumentCountAsync();
        if (count > 0) return; // Skip ingestion if data already exists

        using var httpClient = new HttpClient();
        var response = await httpClient.GetStringAsync("https://api.nobelprize.org/v1/prize.json");

        var jsonData = JsonConvert.DeserializeObject<Root>(response); 

        if (jsonData?.Prizes != null && jsonData.Prizes.Any())
        {
            await _prizeCollection.InsertManyAsync(jsonData.Prizes);
        }
    }
}
public class Laureate
{
    [JsonProperty("id")]
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonProperty("firstname")]
    [JsonPropertyName("firstname")]
    public string Firstname { get; set; }

    [JsonProperty("surname")]
    [JsonPropertyName("surname")]
    public string Surname { get; set; }

    [JsonProperty("motivation")]
    [JsonPropertyName("motivation")]
    public string Motivation { get; set; }

    [JsonProperty("share")]
    [JsonPropertyName("share")]
    public string Share { get; set; }
}

public class Prize
{
    [JsonProperty("year")]
    [JsonPropertyName("year")]
    public string Year { get; set; }

    [JsonProperty("category")]
    [JsonPropertyName("category")]
    public string Category { get; set; }

    [JsonProperty("laureates")]
    [JsonPropertyName("laureates")]
    public List<Laureate> Laureates { get; set; }

    [JsonProperty("overallMotivation")]
    [JsonPropertyName("overallMotivation")]
    public string OverallMotivation { get; set; }
}

public class Root
{
    [JsonProperty("prizes")]
    [JsonPropertyName("prizes")]
    public List<Prize> Prizes { get; set; }
}