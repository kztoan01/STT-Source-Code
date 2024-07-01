using core.Dtos.Album;
using core.Dtos.Artist;
using core.Dtos.Music;
using core.Models;
using core.Objects;
using repository.Repository;
using repository.Repository.Interfaces;
using service.Service.Interfaces;

namespace service.Service;

public class AlbumService : IAlbumService
{
    private readonly IAlbumRepository _albumRepository;
    private readonly IArtistRepository _artistRepository;

    public AlbumService(IAlbumRepository albumRepository, IArtistRepository artistRepository)
    {
        _albumRepository = albumRepository;
        _artistRepository = artistRepository;
    }

    public async Task<Album> CreateAlbumAsync(CreateAlbumDTO albumDTO, Guid artistId)
    {
        var album = new Album
        {
            Id = new Guid(),
            albumTitle = albumDTO.albumTitle,
            artistId = artistId,
            albumDescription = albumDTO.albumDescription,
            releaseDate = albumDTO.releaseDate
        };
        return await _albumRepository.CreateAlbumAsync(album);
    }

    public async Task<bool> DeleteAlbumAsync(Guid albumId)
    {
        return await _albumRepository.DeleteAlbumAsync(albumId);
    }

    public async Task<Album> EditAlbumAsync(CreateAlbumDTO albumDTO, Guid artistId, Guid albumId)
    {
        var album = _albumRepository.GetAlbumById(albumId);
        if (album == null) return null;
        album.Result.albumTitle = albumDTO.albumTitle;
        album.Result.albumDescription = albumDTO.albumDescription;
        album.Result.releaseDate = albumDTO.releaseDate;

        return await _albumRepository.EditAlbumAsync(album.Result);
    }
    public async Task<List<AlbumResponseDTO>> GetAllArtistAlbumsAsync(Guid artistId, QueryObject query)
    {
        var albums = await _albumRepository.GetAllArtistAlbumsAsync(artistId);

        if (!string.IsNullOrWhiteSpace(query.Symbol))
        {
            albums = albums.Where(a => a.albumTitle.Contains(query.Symbol, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        if (!string.IsNullOrWhiteSpace(query.SortBy))
        {
            albums = query.SortBy.Equals("Title", StringComparison.OrdinalIgnoreCase)
                ? query.IsDecsending
                    ? albums.OrderByDescending(a => a.albumTitle).ToList()
                    : albums.OrderBy(a => a.albumTitle).ToList()
                : albums;
        }

        var skipNumber = (query.PageNumber - 1) * query.PageSize;
        var paginatedAlbums = albums.Skip(skipNumber).Take(query.PageSize).ToList();

        var albumResponseDTOs = new List<AlbumResponseDTO>();

        foreach (var album in paginatedAlbums)
        {
            var albumResponseDTO = new AlbumResponseDTO
            {
                Id = album.Id,
                albumTitle = album.albumTitle,
                albumDescription = album.albumDescription,
                releaseDate = album.releaseDate,
                musics = new List<MusicDTO>()
            };

            var musicsList = album.musics;

            foreach (var music in musicsList)
            {
                var musicDTO = new MusicDTO
                {
                    Id = music.Id,
                    musicTitle = music.musicTitle,
                    musicUrl = music.musicUrl,
                    musicPicture = music.musicPicture,
                    musicPlays = music.musicPlays,
                    musicDuration = music.musicDuration,
                    releaseDate = music.releaseDate,
                    genreName = music.genreName,
                    artistName = music.artistName,
                    AlbumDTO = new AlbumDTO
                    {
                        Id = album.Id,
                        albumTitle = album.albumTitle,
                    }
                };

                albumResponseDTO.musics.Add(musicDTO);
            }
            var artistAlbum = await _albumRepository.GetAlbumById(album.Id);
            var artistDTO = await _artistRepository.GetArtistDTOById(artistAlbum.artistId);
            albumResponseDTO.artist = artistDTO;
            albumResponseDTOs.Add(albumResponseDTO);
        }

        return albumResponseDTOs;

    }
    public async Task<List<AlbumResponseDTO>> getAlbumByGenreNameAsync(string genreName, QueryObject query)
    {
        var albums =  await _albumRepository.getAlbumByGenreNameAsync(genreName);
        if (!string.IsNullOrWhiteSpace(query.Symbol))
        {
            albums = albums.Where(a => a.albumTitle.Contains(query.Symbol, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        if (!string.IsNullOrWhiteSpace(query.SortBy))
        {
            albums = query.SortBy.Equals("Title", StringComparison.OrdinalIgnoreCase)
                ? query.IsDecsending
                    ? albums.OrderByDescending(a => a.albumTitle).ToList()
                    : albums.OrderBy(a => a.albumTitle).ToList()
                : albums;
        }

        var skipNumber = (query.PageNumber - 1) * query.PageSize;
        var paginatedAlbums = albums.Skip(skipNumber).Take(query.PageSize).ToList();

        var albumResponseDTOs = new List<AlbumResponseDTO>();

        foreach (var album in paginatedAlbums)
        {
            var albumResponseDTO = new AlbumResponseDTO
            {
                Id = album.Id,
                albumTitle = album.albumTitle,
                albumDescription = album.albumDescription,
                releaseDate = album.releaseDate,
                musics = new List<MusicDTO>()
            };

            var musicsList = album.musics;

            foreach (var music in musicsList)
            {
                var musicDTO = new MusicDTO
                {
                    Id = music.Id,
                    musicTitle = music.musicTitle,
                    musicUrl = music.musicUrl,
                    musicPicture = music.musicPicture,
                    musicPlays = music.musicPlays,
                    musicDuration = music.musicDuration,
                    releaseDate = music.releaseDate,
                    genreName = music.genreName,
                    artistName = music.artistName,
                    AlbumDTO = new AlbumDTO
                    {
                        Id = album.Id,
                        albumTitle = album.albumTitle,
                    }
                };

                albumResponseDTO.musics.Add(musicDTO);
            }
            var artistAlbum = await _albumRepository.GetAlbumById(album.Id);
            var artistDTO = await _artistRepository.GetArtistDTOById(artistAlbum.artistId);
            albumResponseDTO.artist = artistDTO;
            albumResponseDTOs.Add(albumResponseDTO);
        }

        return albumResponseDTOs;
    }

    public async Task<Album> GetAlbumByIdAsync(Guid albumId)
    {
        return await _albumRepository.GetAlbumById(albumId);
    }

    public async Task<AlbumResponseDTO> GetAlbumDetail(Guid albumId)
    {
        return await _albumRepository.GetAlbumDetails(albumId);
    }

    public async Task<List<AlbumResponseDTO>> getAllAlbumsAsync(QueryObject query)
    {
        var albums = await _albumRepository.getAllAlbumsAsync();

        if (!string.IsNullOrWhiteSpace(query.Symbol))
        {
            albums = albums.Where(a => a.albumTitle.Contains(query.Symbol, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        if (!string.IsNullOrWhiteSpace(query.SortBy))
        {
            albums = query.SortBy.Equals("Title", StringComparison.OrdinalIgnoreCase)
                ? query.IsDecsending
                    ? albums.OrderByDescending(a => a.albumTitle).ToList()
                    : albums.OrderBy(a => a.albumTitle).ToList()
                : albums;
        }

        var skipNumber = (query.PageNumber - 1) * query.PageSize;
        var paginatedAlbums = albums.Skip(skipNumber).Take(query.PageSize).ToList();

        var albumResponseDTOs = new List<AlbumResponseDTO>();

        foreach (var album in paginatedAlbums)
        {
            var albumResponseDTO = new AlbumResponseDTO
            {
                Id = album.Id,
                albumTitle = album.albumTitle,
                albumDescription = album.albumDescription,
                releaseDate = album.releaseDate,
                musics = new List<MusicDTO>()
            };

            var musicsList = album.musics;

            foreach (var music in musicsList)
            {
                var musicDTO = new MusicDTO
                {
                    Id = music.Id,
                    musicTitle = music.musicTitle,
                    musicUrl = music.musicUrl,
                    musicPicture = music.musicPicture,
                    musicPlays = music.musicPlays,
                    musicDuration = music.musicDuration,
                    releaseDate = music.releaseDate,
                    genreName = music.genreName,
                    artistName = music.artistName,
                    AlbumDTO = new AlbumDTO
                    {
                        Id = album.Id,
                        albumTitle = album.albumTitle,
                    }
                };

                albumResponseDTO.musics.Add(musicDTO);
            }
            var artistAlbum = await _albumRepository.GetAlbumById(album.Id);
            var artistDTO = await _artistRepository.GetArtistDTOById(artistAlbum.artistId);
            albumResponseDTO.artist = artistDTO;
            albumResponseDTOs.Add(albumResponseDTO);
        }

        return albumResponseDTOs;
    }



    public async Task<Album> GetMostListenAlbum()
    {
        return await _albumRepository.GetMostListenAlbum();
    }
}