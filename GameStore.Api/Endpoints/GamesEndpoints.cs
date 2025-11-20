using GameStore.Api.Contracts;

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
        var group = app.MapGroup("games");

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
        group.MapPost("/", (CreateGameContract newGame) =>
            {
                GameContract game = new(
                    GAMES.Count + 1,
                    newGame.Name,
                    newGame.Genre,
                    newGame.Price,
                    newGame.ReleaseDate
                );

                GAMES.Add(game);

                return Results.CreatedAtRoute(GET_GAME_ENDPOINT_NAME, new { id = game.Id }, game);
            })
            .WithParameterValidation();

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