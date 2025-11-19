using GameStore.Api.Contracts;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const string getGameEndpointName = "GetGame";

List<GameContract> games =
[
    new(1, "Sonic Racing Cross Worlds", "Racing", 69.99M, new DateOnly(2025, 9, 25)),
    new(2, "Street Fighter II", "Fighting", 19.99M, new DateOnly(1992, 7, 15)),
    new(3, "Final Fantasy XIV", "RPG", 59.99M, new DateOnly(2010, 9, 30)),
    new(4, "FIFA 23", "Sports", 69.99M, new DateOnly(2022, 9, 27))
];

// GET /games
app.MapGet("games", () => games);

// GET /games/1
app.MapGet("games/{id}", (int id) => games.Find(game => game.Id == id))
    .WithName(getGameEndpointName);

// POST /games
app.MapPost("games", (CreateGameContract newGame) =>
{
    GameContract game = new(
        games.Count + 1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate
    );

    games.Add(game);

    return Results.CreatedAtRoute(getGameEndpointName, new { id = game.Id }, game);
});

app.Run();