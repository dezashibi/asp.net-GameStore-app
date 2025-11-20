using GameStore.Api.Contracts;
using GameStore.Api.Data;
using GameStore.Api.Entities;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    private const string GET_GAME_ENDPOINT_NAME = "GetGame";

    private static readonly List<GameContract> GAMES =
    [
        new(1, "Sonic Racing Cross Worlds", "Racing", 69.99M, new DateOnly(2025, 9, 25)),
        new(2, "Street Fighter II", "Fighting", 19.99M, new DateOnly(1992, 7, 15)),
        new(3, "Final Fantasy XIV", "RPG", 59.99M, new DateOnly(2010, 9, 30)),
        new(4, "FIFA 23", "Sports", 69.99M, new DateOnly(2022, 9, 27))
    ];

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games").WithParameterValidation();

        // GET /games
        group.MapGet("/", () => GAMES);

        // GET /games/id
        group.MapGet("/{id}", (int id) =>
            {
                var game = GAMES.Find(game => game.Id == id);

                return game is null ? Results.NotFound() : Results.Ok(game);
            })
            .WithName(GET_GAME_ENDPOINT_NAME);

        // POST /games
        group.MapPost("/", (CreateGameContract newGame, GameStoreContext dbContext) =>
        {
            var game = new Game
            {
                Name = newGame.Name,
                Genre = dbContext.Genres.Find(newGame.GenreId),
                GenreId = newGame.GenreId,
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate
            };

            dbContext.Games.Add(game);
            dbContext.SaveChanges();

            // Keep the contract of response to the end user
            var gameContract = new GameContract(
                game.Id,
                game.Name,
                game.Genre?.Name ?? "N/A",
                game.Price,
                game.ReleaseDate
            );

            return Results.CreatedAtRoute(GET_GAME_ENDPOINT_NAME, new { id = game.Id }, gameContract);
        });

        // PUT /games/id
        group.MapPut("/{id}", (int id, UpdateGameContract updatedGame) =>
        {
            var index = GAMES.FindIndex(game => game.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }

            GAMES[index] = new GameContract(
                id,
                updatedGame.Name,
                updatedGame.Genre,
                updatedGame.Price,
                updatedGame.ReleaseDate
            );

            return Results.NoContent();
        });

        // DELETE /games/id
        group.MapDelete("/{id}", (int id) =>
        {
            GAMES.RemoveAll(game => game.Id == id);

            return Results.NoContent();
        });

        return group;
    }
}