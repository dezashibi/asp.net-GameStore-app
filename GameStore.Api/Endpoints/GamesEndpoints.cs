using GameStore.Api.Contracts;
using GameStore.Api.Data;
using GameStore.Api.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    private const string GET_GAME_ENDPOINT_NAME = "GetGame";

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games").WithParameterValidation();

        // GET /games
        group.MapGet("/",
            (GameStoreContext dbContext) => dbContext.Games
                .Include(game => game.Genre)
                .Select(game => game.ToSummaryContract())
                .AsNoTracking()
        );

        // GET /games/id
        group.MapGet("/{id}", (int id, GameStoreContext dbContext) =>
            {
                var game = dbContext.Games.Find(id);

                return game is null ? Results.NotFound() : Results.Ok(game.ToDetailsContract());
            })
            .WithName(GET_GAME_ENDPOINT_NAME);

        // POST /games
        group.MapPost("/", (CreateGameContract newGame, GameStoreContext dbContext) =>
        {
            var game = newGame.ToEntity();

            dbContext.Games.Add(game);
            dbContext.SaveChanges();

            return Results.CreatedAtRoute(GET_GAME_ENDPOINT_NAME, new { id = game.Id }, game.ToDetailsContract());
        });

        // PUT /games/id
        group.MapPut("/{id}", (int id, UpdateGameContract updatedGame, GameStoreContext dbContext) =>
        {
            var existingGame = dbContext.Games.Find(id);

            if (existingGame is null)
            {
                return Results.NotFound();
            }

            dbContext
                .Entry(existingGame)
                .CurrentValues
                .SetValues(updatedGame.ToEntity(id));

            dbContext.SaveChanges();

            return Results.NoContent();
        });

        // DELETE /games/id
        group.MapDelete("/{id}", (int id, GameStoreContext dbContext) =>
        {
            dbContext.Games
                .Where(game => game.Id == id)
                .ExecuteDelete();

            return Results.NoContent();
        });

        return group;
    }
}