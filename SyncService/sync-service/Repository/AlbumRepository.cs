using Microsoft.EntityFrameworkCore;
using sync_service.Data;
using sync_service.Dtos.Album;
using sync_service.Interfaces;

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

    }
}
