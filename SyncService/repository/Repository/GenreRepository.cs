using core.Models;
using data.Data;
using Microsoft.EntityFrameworkCore;
using repository.Repository.Interfaces;

namespace repository.Repository;

public class GenreRepository : IGenreRepository
{
    private readonly ApplicationDBContext _context;

    public GenreRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<List<Genre>> GetAllGenre()
    {
        return await _context.Genres.ToListAsync();
    }
}