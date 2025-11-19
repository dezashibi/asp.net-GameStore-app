namespace GameStore.Api.Contracts;

public record CreateGameContract(string Name, string Genre, decimal Price, DateOnly ReleaseDate);