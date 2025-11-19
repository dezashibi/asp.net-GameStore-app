namespace GameStore.Api.Contracts;

public record GameContract(int Id, string Name, string Genre, decimal Price, DateOnly ReleaseDate);