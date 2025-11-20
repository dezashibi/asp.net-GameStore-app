using GameStore.Api.Contracts;
using GameStore.Api.Entities;

namespace GameStore.Api.Mapping;

public static class GameMapping
{
    public static Game ToEntity(this CreateGameContract gameContract)
    {
        return new Game
        {
            Name = gameContract.Name,
            GenreId = gameContract.GenreId,
            Price = gameContract.Price,
            ReleaseDate = gameContract.ReleaseDate
        };
    }

    public static GameContract ToContract(this Game game)
    {
        return new GameContract(
            game.Id,
            game.Name,
            game.Genre?.Name ?? "N/A",
            game.Price,
            game.ReleaseDate
        );
    }
}