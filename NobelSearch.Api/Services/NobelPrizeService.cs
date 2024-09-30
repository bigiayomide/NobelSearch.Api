using MongoDB.Bson;
using MongoDB.Driver;
using NobelSearch.Api.Models;

namespace NobelSearch.Api.Services
{
    public interface INobelPrizeService
    {
        Task<List<Prize>> SearchByNameAsync(string query);
        Task<List<Prize>> SearchByCategoryAsync(string query);
        Task<List<Prize>> SearchByDescriptionAsync(string query);
    }

    public class NobelPrizeService : INobelPrizeService
    {
        private readonly IMongoCollection<Prize> _prizeCollection;

        public NobelPrizeService(IDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _prizeCollection = database.GetCollection<Prize>(settings.NobelPrizeCollectionName);
        }

        public async Task<List<Prize>> SearchByNameAsync(string query)
        {
            var prizes = await _prizeCollection.Find(_ => true).ToListAsync();

            var results = prizes.Where(p =>
                p.Laureates != null && p.Laureates.Any(l =>
                    FuzzySharp.Fuzz.Ratio($"{l.Firstname} {l.Surname}", query) > 70
                )
            ).ToList();

            return results;
        }

        public async Task<List<Prize>> SearchByCategoryAsync(string query)
        {
            var filter = Builders<Prize>.Filter.Regex("category", new BsonRegularExpression(query, "i"));
            return await _prizeCollection.Find(filter).ToListAsync();
        }

        public async Task<List<Prize>> SearchByDescriptionAsync(string query)
        {
            var prizes = await _prizeCollection.Find(_ => true).ToListAsync();

            var results = prizes.Where(p =>
                p.Laureates != null && p.Laureates.Any(l =>
                    FuzzySharp.Fuzz.PartialRatio(l.Motivation, query) > 70
                )
            ).ToList();

            return results;
        }
    }
}
