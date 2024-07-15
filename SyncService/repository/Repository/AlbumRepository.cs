using core.Dtos.Album;
using core.Dtos.Artist;
using core.Dtos.Music;
using core.Models;
using data.Data;
using Microsoft.EntityFrameworkCore;
using repository.Repository.Interfaces;

namespace repository.Repository;


public class AlbumRepository : IAlbumRepository
{
    private readonly ApplicationDBContext _context;
    private readonly IArtistRepository _artistRepository;

    public AlbumRepository(ApplicationDBContext context, IArtistRepository artistRepository)
    {
        _context = context;
        _artistRepository = artistRepository;
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

    public async Task<List<AlbumResponseDTO>> getAlbumByGenreNameAsync(string genreName)
    {
        // handle mapper later
        var listAlbum = _context.Albums
            .Where(a => a.Musics
                .Any(m => m.Genre.genreName == genreName))
            .ToList();
        List<AlbumResponseDTO> albumList = new List<AlbumResponseDTO>();
        foreach (var album in listAlbum)
        {
            var albumRes = await GetAlbumDetails(album.Id);
            albumList.Add(albumRes);
        } 
         return albumList;
    }

    public async Task<Album> GetAlbumById(Guid albumId)
    {
        return await _context.Albums
            .Include(a => a.Musics)
                .ThenInclude(m => m.Artist)
            .FirstOrDefaultAsync(a => a.Id == albumId);
    }

    public async Task<List<AlbumResponseDTO>> GetAllArtistAlbumsAsync(Guid artistId)
    {
        var albumList = await _context.Albums
            .Where(a => a.artistId == artistId)
            .ToListAsync();
        List<AlbumResponseDTO> result = new List<AlbumResponseDTO>();
        foreach (var album in albumList)
        {
            var albumRes = GetAlbumDetails(album.Id);
            result.Add(albumRes.Result);
        }
        return result;
    }

    public async Task<AlbumResponseDTO> GetAlbumDetails(Guid albumId)
    {
        var album = await _context.Albums
            .Include(a =>a.Musics)
                .ThenInclude(m =>m.Genre)
            .Include(a => a.Musics)
                .ThenInclude(m => m.Artist)
                  .ThenInclude(a => a.User)
            .FirstOrDefaultAsync(a => a.Id == albumId);

        var artistDTO = await _artistRepository.GetArtistDTOById(album.artistId);
        List<MusicDTO> listMusic = new List<MusicDTO>();

        foreach(var music in album.Musics)
        {
            var albumMusic = new AlbumDTO
            {
                albumTitle = music.Album.albumTitle,
                albumDescription = music.Album.albumDescription,
                releaseDate = music.Album.releaseDate,
                Id = (Guid)music.albumId
            };
            var musicDTO = new MusicDTO
            {
                Id = music.Id,
                artistName = music.Artist.User.userFullName,
                genreName = music.Genre.genreName,
                musicDuration = music.musicDuration,
                musicPicture = music.musicPicture,
                musicPlays = music.musicPlays,
                musicTitle = music.musicTitle,
                musicUrl = music.musicUrl,
                releaseDate = music.releaseDate,
                AlbumDTO = albumMusic
            };
            listMusic.Add(musicDTO);
        }
      
        var albumDTO = new AlbumResponseDTO
        {
            Id = album.Id,
            albumTitle = album.albumTitle,
            releaseDate = album.releaseDate,
            albumDescription = album.albumDescription,
            artist = artistDTO,
            musics = listMusic
        };
        return albumDTO;
    }

    public async Task<List<AlbumResponseDTO>> getAllAlbumsAsync()
    {
        var albumList = await _context.Albums.ToListAsync();
        List<AlbumResponseDTO> list = new List<AlbumResponseDTO>();
        foreach(var album in albumList)
        {
            var albumRes = await GetAlbumDetails(album.Id);
            list.Add(albumRes);
        }
        return list;
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