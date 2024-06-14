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

        public async Task<List<AlbumDTO>> GetAlbumByContainArtistByArtistId(Guid artistId)
        {
            var albums = await _context.Albums
                .Where(a => a.artistId == artistId)
                .ToListAsync();

 
            var albumDTOs = albums.Select(album => new AlbumDTO
            {
                Id = album.Id,
                albumTitle = album.albumTitle,
                releaseDate = album.releaseDate,
                albumDescription = album.albumDescription,
            }).ToList();

            return albumDTOs;
        }

        public async Task<List<Album>> getAlbumByGenreNameAsync(string genreName)
        {
            // handle mapper later
            return await _context.Albums
                .Include(a =>a.Musics)
                .Where(a => a.Musics
                .Any(m => m.Genre.genreName == genreName))
                .ToListAsync();
        }

        public async Task<List<Album>> getAllAlbumsAsync()
        {
            return await _context.Albums
                .Include (a => a.Musics)
                .ToListAsync();
        }
    }
}
