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
            async (GameStoreContext dbContext) =>
            {
                // await Task.Delay(3000); // Simulate some latency in the server

                return await dbContext.Games
                    .Include(game => game.Genre)
                    .Select(game => game.ToSummaryContract())
                    .AsNoTracking()
                    .ToListAsync();
            }
        );

        // GET /games/id
        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
            {
                var game = await dbContext.Games.FindAsync(id);

                return game is null ? Results.NotFound() : Results.Ok(game.ToDetailsContract());
            })
            .WithName(GET_GAME_ENDPOINT_NAME);

        // POST /games
        group.MapPost("/", async (CreateGameContract newGame, GameStoreContext dbContext) =>
        {
            var game = newGame.ToEntity();

            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();

            return Results.CreatedAtRoute(GET_GAME_ENDPOINT_NAME, new { id = game.Id }, game.ToDetailsContract());
        });

        // PUT /games/id
        group.MapPut("/{id}", async (int id, UpdateGameContract updatedGame, GameStoreContext dbContext) =>
        {
            var existingGame = await dbContext.Games.FindAsync(id);

            if (existingGame is null)
            {
                return Results.NotFound();
            }

            dbContext
                .Entry(existingGame)
                .CurrentValues
                .SetValues(updatedGame.ToEntity(id));

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        // DELETE /games/id
        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            await dbContext.Games
                .Where(game => game.Id == id)
                .ExecuteDeleteAsync();

            return Results.NoContent();
        });

        return group;
    }
}