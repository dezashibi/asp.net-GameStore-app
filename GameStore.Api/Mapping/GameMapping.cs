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

    public static Game ToEntity(this UpdateGameContract gameContract, int id)
    {
        return new Game
        {
            Id = id,
            Name = gameContract.Name,
            GenreId = gameContract.GenreId,
            Price = gameContract.Price,
            ReleaseDate = gameContract.ReleaseDate
        };
    }

    public static GameSummaryContract ToSummaryContract(this Game game)
    {
        return new GameSummaryContract(
            game.Id,
            game.Name,
            game.Genre?.Name ?? "N/A",
            game.Price,
            game.ReleaseDate
        );
    }

    public static GameDetailsContract ToDetailsContract(this Game game)
    {
        return new GameDetailsContract(
            game.Id,
            game.Name,
            game.GenreId,
            game.Price,
            game.ReleaseDate
        );
    }
}