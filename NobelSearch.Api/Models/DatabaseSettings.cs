namespace NobelSearch.Api.Models
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string NobelPrizeCollectionName { get; set; } = "prizes";
        public string ConnectionString { get; set; } = "mongodb://mongo:27017";
        public string DatabaseName { get; set; } = "nobel_prize_db";
    }

    public interface IDatabaseSettings
    {
        string NobelPrizeCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
