using GameStore.Api.Data;
using GameStore.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var connStr = builder.Configuration.GetConnectionString("GameStore");
builder.Services.AddSqlite<GameStoreContext>(connStr);

var app = builder.Build();

app.MapGamesEndpoints();

await app.MigrateDbAsync();

app.Run();