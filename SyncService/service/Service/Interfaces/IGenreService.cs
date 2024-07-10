using core.Models;

namespace service.Service.Interfaces;

public interface IGenreService
{
    Task<List<Genre>> GetAllGenre();
}