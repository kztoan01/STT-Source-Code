using core.Dtos.Album;
using core.Dtos.Artist;
using core.Dtos.Music;
using core.Models;
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
            ArtistName = artist.User.userFullName,
            artistImage = artist.ImageUrl,
            artistDescription = artist.artistDescription,
            NumberOfFollower = artist.Followers.Count,
            Albums = artist.Albums.Select(a => new AlbumDTO
            {
                Id = a.Id,
                albumTitle = a.albumTitle,
                albumDescription = a.albumDescription,
                releaseDate = a.releaseDate,
                imageUrl = a.ImageUrl
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
                releaseDate = m.releaseDate,
                musicPlays = m.musicPlays,
                genreName = m.Genre.genreName,
                albumTitle = m.Album.albumTitle
            })
            .ToListAsync();
    }

    public async Task<Artist> GetArtistByUserId(Guid userId)
    {
        return await _context.Artists.FirstOrDefaultAsync(a => a.userId == userId.ToString());
    }

    public async Task<Artist> CreateArtist(Artist artist)
    {
        await _context.Artists.AddAsync(artist);
        await _context.SaveChangesAsync();
        return artist;
    }

    public async Task<bool> updateArtist(Artist artist)
    {
        _context.Artists.Update(artist);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<ArtistDTO>> GetAllArtistDTOs()
    {
        var artist = await _context.Artists
            .Include(a => a.User).ThenInclude(user => user.Followers)
            .Include(a => a.Musics)
            .ThenInclude(m => m.Genre)
            .Include(a => a.Musics)
            .ThenInclude(m => m.Album)
            .Include(a => a.Musics)
            .ThenInclude(m => m.playlistMusics)
            .Include(a => a.Albums)
            .ToListAsync();
        var listArtistDtOs = new List<ArtistDTO>();
        foreach (var tmp in artist)
        {
            var artistDTO = new ArtistDTO
            {
                Id = tmp.Id,
                userId = tmp.userId,
                ArtistName = tmp.User.userFullName,
                artistImage = tmp.ImageUrl,
                artistDescription = tmp.artistDescription,
                NumberOfFollower = tmp.User.Followers.Count,
                Albums = tmp.Albums.Select(a => new AlbumDTO
                {
                    Id = a.Id,
                    albumTitle = a.albumTitle,
                    albumDescription = a.albumDescription,
                    releaseDate = a.releaseDate,
                    imageUrl = a.ImageUrl
                }).ToList(),
                ViralMusics = tmp.Musics.Select(m => new MusicDTO
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
            listArtistDtOs.Add(artistDTO);
        }

        return listArtistDtOs;
    }

    public async Task<bool> DeleteArtist(Guid id)
    {
        var artist = await _context.Artists
            .Include(a => a.Albums)
            .Include(a => a.Musics)
            .Include(a => a.Followers)
            .FirstOrDefaultAsync(a => a.Id == id);
        if (artist == null) return false;

        // Remove related entities
        _context.Albums.RemoveRange(artist.Albums);
        _context.Musics.RemoveRange(artist.Musics);
        _context.Follower.RemoveRange(artist.Followers);
        // Remove the artist
        _context.Artists.Remove(artist);

        // Save changes to the database
        await _context.SaveChangesAsync();
        return true;
    }
}