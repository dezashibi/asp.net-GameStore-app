using GameStore.Frontend.Models;

namespace GameStore.Frontend.Clients;

public class GamesClient
{
    private readonly List<GameSummary> _games =
    [
        new() { Id = 1, Name = "Sonic Racing Cross Worlds", Genre = "Racing", Price = 69.99M, ReleaseDate = new DateOnly(2025, 9, 25) },
        new() { Id = 2, Name = "Street Fighter II", Genre = "Fighting", Price = 19.99M, ReleaseDate = new DateOnly(1992, 7, 15) },
        new() { Id = 3, Name = "Final Fantasy XIV", Genre = "RPG", Price = 59.99M, ReleaseDate = new DateOnly(2010, 9, 30) },
        new() { Id = 4, Name = "FIFA 23", Genre = "Sports", Price = 69.99M, ReleaseDate = new DateOnly(2022, 9, 27) }
    ];

    public GameSummary[] GetGames()
    {
        return [.._games];
    }
}