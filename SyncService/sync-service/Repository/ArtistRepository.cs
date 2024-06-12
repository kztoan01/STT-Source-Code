using Microsoft.EntityFrameworkCore;
using sync_service.Data;
using sync_service.Dtos.Artist;
using sync_service.Interfaces;
using sync_service.Models;

namespace sync_service.Repository
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly ApplicationDBContext _context;
        public ArtistRepository (ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<AlbumResponseDTO>> GetAllArtistAlbumsAsync(Guid artistId)
        {
            return await _context.Albums
                .Where(a => a.artistId == artistId)
                .Select(a => new AlbumResponseDTO
                {
                    Id = a.Id,
                    albumTitle = a.albumTitle,
                    albumDescription = a.albumDescription,
                    releaseDate = a.releaseDate,
                })
                .ToListAsync();
        }

        public async Task<List<MusicResponseDTO>> GetAllArtistMusicsAsync(Guid artistId)
        {
            return await _context.Musics
                .Where(m => m.artistId.Equals(artistId))
                .Select( m => new MusicResponseDTO
                {
                    Id=m.Id,
                    musicTitle = m.musicTitle,
                    musicUrl = m.musicUrl,
                    musicPicture = m.musicPicture,
                    musicPlays = m.musicPlays,
                    genreName = m.Genre.genreName,
                    albumTitle = m.Album.albumTitle
                })
                .ToListAsync();

        }
    }
}
