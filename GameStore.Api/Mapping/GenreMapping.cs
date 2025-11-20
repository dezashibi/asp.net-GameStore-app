using GameStore.Api.Contracts;
using GameStore.Api.Entities;

namespace GameStore.Api.Mapping;

public static class GenreMapping
{
    public static GenreContract ToContract(this Genre genre)
    {
        return new GenreContract(genre.Id, genre.Name);
    }
}