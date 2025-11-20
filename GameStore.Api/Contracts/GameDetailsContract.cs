namespace GameStore.Api.Contracts;

public record GameDetailsContract(int Id, string Name, int GenreId, decimal Price, DateOnly ReleaseDate);