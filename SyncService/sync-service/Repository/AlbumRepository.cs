using Microsoft.EntityFrameworkCore;
using sync_service.Data;
using sync_service.Interfaces;
using sync_service.Models;

namespace sync_service.Repository
{
    public class AlbumRepository : IAlbumRepository
    {
        private readonly ApplicationDBContext _context;

        public AlbumRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<Album>> getAlbumByGenreNameAsync(string genreName)
        {
            // handle mapper later
            return await _context.Albums
                .Where(a => a.Musics
                .Any(m => m.Genre.genreName == genreName))
                .ToListAsync();
        }
    }
}
