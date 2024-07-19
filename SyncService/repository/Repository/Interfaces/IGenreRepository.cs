using core.Models;

namespace repository.Repository.Interfaces;

public interface IGenreRepository
{
    Task<List<Genre>> GetAllGenre();
    Task<Genre> GetGenreById(Guid id);
    Task<Genre> CreateGenreAsync(Genre genre);
    Task<Genre> UpdateGenreAsync(Genre genre);
    Task<bool> DeleteGenreByIdAsync(Guid id);
}