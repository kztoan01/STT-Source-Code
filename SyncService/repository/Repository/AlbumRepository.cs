using core.Dtos.Album;
using core.Dtos.Artist;
using core.Models;
using data.Data;
using Microsoft.EntityFrameworkCore;
using repository.Repository.Interfaces;

namespace repository.Repository;


public class AlbumRepository : IAlbumRepository
{
    private readonly ApplicationDBContext _context;

    public AlbumRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<Album> CreateAlbumAsync(Album album)
    {
        _context.Albums.Add(album);
        await _context.SaveChangesAsync();
        return album;
    }

    public async Task<bool> DeleteAlbumAsync(Guid albumId)
    {
        var album = await _context.Albums.FirstOrDefaultAsync(a => a.Id == albumId);

        if (album == null)
        {
            return false;
        }

        _context.Albums.Remove(album);
        await _context.SaveChangesAsync();

        return true; 
    }

    public async Task<Album> EditAlbumAsync(Album album)
    {
        _context.Albums.Update(album);
        await _context.SaveChangesAsync();
        return album;
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
            albumDescription = album.albumDescription
        }).ToList();

        return albumDTOs;
    }

    public async Task<List<Album>> getAlbumByGenreNameAsync(string genreName)
    {
        // handle mapper later
        return await _context.Albums
            .Include(a => a.Musics)
            .Where(a => a.Musics
                .Any(m => m.Genre.genreName == genreName))
            .ToListAsync();
    }

    public async Task<Album> GetAlbumById(Guid albumId)
    {
        return await _context.Albums.FirstOrDefaultAsync(a => a.Id == albumId);
    }

    public async Task<AlbumResponseDTO> GetAlbumDetails(Guid albumId)
    {
        var album = await _context.Albums
            .Include(a => a.Musics)
            .FirstOrDefaultAsync(a => a.Id == albumId);

        var artistId = album.artistId;

        var artist = await _context.Artists
            .FirstOrDefaultAsync(a => a.Id == artistId);
        ArtistDTO artistDTO = new ArtistDTO
        {
            Id = artist.Id,
            AristName = "handle later",
            artistDescription = artist.artistDescription,
        };
        var albumDTO = new AlbumResponseDTO
        {
            Id = album.Id,
            albumTitle = album.albumTitle,
            releaseDate = album.releaseDate,
            albumDescription = album.albumDescription,
            artist = artistDTO
        };
        return albumDTO;
    }

    public async Task<List<Album>> getAllAlbumsAsync()
    {
        return await _context.Albums
            .Include(a => a.Musics)
            .ToListAsync();
    }

    public async Task<Album> GetMostListenAlbum()
    {
        var albumList = await _context.Albums.Include(a => a.Musics).ToListAsync();

        if (albumList == null) return null;

        var mostListenAlbum = albumList
            .Select(album => new
            {
                Album = album,
                TotalPlays = album.Musics.Sum(music => music.musicPlays)
            })
            .OrderByDescending(albumWithPlays => albumWithPlays.TotalPlays)
            .FirstOrDefault();

        // Return the album with the most plays
        return mostListenAlbum?.Album;
    }
}