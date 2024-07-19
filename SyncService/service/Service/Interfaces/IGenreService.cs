using core.Dtos.Genre;
using core.Models;
using core.Objects;

namespace service.Service.Interfaces;

public interface IGenreService
{
    Task<List<Genre>> GetAllGenre(QueryObject query);
    Task<Genre> CreateGenreAsync(GenreDTO genre);
    Task<Genre> GetGenreByIdAsync(Guid id);
    Task<Genre> UpdateGenreByIdAsync(GenreDTO genre, Guid id);
    Task<bool> DeleteGenreByIdAsync(Guid id);
}