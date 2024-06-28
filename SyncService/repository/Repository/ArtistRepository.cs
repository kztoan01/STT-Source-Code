using core.Dtos.Album;
using core.Dtos.Artist;
using core.Dtos.Music;
using data.Data;
using Microsoft.EntityFrameworkCore;
using repository.Repository.Interfaces;

namespace repository.Repository;

public class ArtistRepository : IArtistRepository
{
    private readonly ApplicationDBContext _context;

    public ArtistRepository(ApplicationDBContext context)
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
                releaseDate = a.releaseDate
            })
            .ToListAsync();
    }

    public async Task<ArtistDTO> GetArtistDTOById(Guid id)
    {
        var artist = await _context.Artists
            .Include(a => a.User)
            .Include(a => a.Musics)
            .ThenInclude(m => m.Genre)
            .Include(a => a.Musics)
            .ThenInclude(m => m.Album)
            .Include(a => a.Musics)
            .ThenInclude(m => m.playlistMusics)
            .Include(a => a.Albums)
            .Include(a => a.Followers)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (artist == null) return null;

        var artistDTO = new ArtistDTO
        {
            Id = artist.Id,
            userId = artist.userId,
            AristName = artist.User.UserName,
            artistDescription = artist.artistDescription,
            NumberOfFollower = artist.Followers.Count,
            Albums = artist.Albums.Select(a => new AlbumDTO
            {
                Id = a.Id,
                albumTitle = a.albumTitle
            }).ToList(),
            ViralMusics = artist.Musics.Select(m => new MusicDTO
            {
                Id = m.Id,
                genreName = m.Genre.genreName,
                musicDuration = m.musicDuration,
                musicPicture = m.musicPicture,
                musicPlays = m.musicPlays,
                musicTitle = m.musicTitle,
                musicUrl = m.musicUrl,
                releaseDate = m.releaseDate
            }).ToList()
        };

        return artistDTO;
    }

    public async Task<List<MusicResponseDTO>> GetAllArtistMusicsAsync(Guid artistId)
    {
        return await _context.Musics
            .Where(m => m.artistId.Equals(artistId))
            .Select(m => new MusicResponseDTO
            {
                Id = m.Id,
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