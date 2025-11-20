namespace GameStore.Api.Contracts;

public record GameSummaryContract(int Id, string Name, string Genre, decimal Price, DateOnly ReleaseDate);