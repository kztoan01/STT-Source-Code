using core.Dtos.Album;
using core.Models;
using core.Objects;
using repository.Repository;
using repository.Repository.Interfaces;
using service.Service.Interfaces;

namespace service.Service;

public class AlbumService : IAlbumService
{
    private readonly IAlbumRepository _albumRepository;

    public AlbumService(IAlbumRepository albumRepository)
    {
        _albumRepository = albumRepository;
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
    public async Task<List<AlbumResponseDTO>> GetAllArtistAlbumsAsync(Guid artistId)
    {
        return await _albumRepository.GetAllArtistAlbumsAsync(artistId);
    }
    public async Task<List<AlbumResponseDTO>> getAlbumByGenreNameAsync(string genreName)
    {
        return await _albumRepository.getAlbumByGenreNameAsync(genreName);
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
        //var albums = await _albumRepository.getAllAlbumsAsync();

        //if (!string.IsNullOrWhiteSpace(query.SortBy))
        //    albums = query.SortBy.Equals("Name", StringComparison.OrdinalIgnoreCase)
        //        ? query.IsDecsending
        //            ? albums.OrderByDescending(a => a.albumTitle).ToList()
        //            : albums.OrderBy(a => a.albumTitle).ToList()
        //        : albums;

        //var skipNumber = (query.PageNumber - 1) * query.PageSize;
        //var paginatedAlbums = albums.Skip(skipNumber).Take(query.PageSize).ToList();

        return await _albumRepository.getAllAlbumsAsync();
    }

    public async Task<Album> GetMostListenAlbum()
    {
        return await _albumRepository.GetMostListenAlbum();
    }
}