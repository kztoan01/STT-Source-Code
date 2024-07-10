using core.Models;

namespace repository.Repository.Interfaces;

public interface IGenreRepository
{
    Task<List<Genre>> GetAllGenre();
}