using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NobelSearch.Api.Models;
using NobelSearch.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection(nameof(DatabaseSettings)));

builder.Services.AddSingleton<IDatabaseSettings>(sp =>
    sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);

builder.Services.AddSingleton<IMongoClient>(s =>
    new MongoClient(builder.Configuration.GetValue<string>("DatabaseSettings:ConnectionString")));

builder.Services.AddScoped<INobelPrizeService, NobelPrizeService>();
builder.Services.AddScoped<DataIngestionService>();

builder.Services.AddControllers();

var app = builder.Build();

// Ingest data on startup
using (var scope = app.Services.CreateScope())
{
    var dataIngestion = scope.ServiceProvider.GetRequiredService<DataIngestionService>();
    await dataIngestion.IngestDataAsync();
}

app.MapControllers();
app.Run();