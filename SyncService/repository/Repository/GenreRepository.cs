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

    public async Task<Genre> CreateGenreAsync(Genre genre)
    {
        _context.Genres.AddAsync(genre); 
        await _context.SaveChangesAsync();
        return genre;
    }

    public async Task<bool> DeleteGenreByIdAsync(Guid id)
    {
        var genre = await _context.Genres.FirstOrDefaultAsync(g => g.Id == id);
        if (genre == null)
        {
            return false;
        }
        _context.Genres.Remove(genre);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Genre>> GetAllGenre()
    {
        return await _context.Genres.ToListAsync();
    }

    public async Task<Genre> GetGenreById(Guid id)
    {
        var genre = await _context.Genres.FirstOrDefaultAsync(g => g.Id == id);
        return genre;
    }

    public async Task<Genre> UpdateGenreAsync(Genre genre)
    {
       _context.Genres.Update(genre);
        await _context.SaveChangesAsync();
        return genre;
    }
}