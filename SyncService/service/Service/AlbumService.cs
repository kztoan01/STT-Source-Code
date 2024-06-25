using core.Dtos.Album;
using core.Models;
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
        Album album = new Album
        {
            Id = new Guid(),
            albumTitle = albumDTO.albumTitle,
            artistId = artistId,
            albumDescription = albumDTO.albumDescription,
            releaseDate = albumDTO.releaseDate,
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
        if (album == null)
        {
            return null;
        }
        album.Result.albumTitle = albumDTO.albumTitle;
        album.Result.albumDescription = albumDTO.albumDescription;
        album.Result.releaseDate = albumDTO.releaseDate;

        return await _albumRepository.EditAlbumAsync(album.Result);
    }

    public async Task<List<Album>> getAlbumByGenreNameAsync(string genreName)
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

    public async Task<List<Album>> getAllAlbumsAsync()
    {
        return await _albumRepository.getAllAlbumsAsync();
    }

    public async Task<Album> GetMostListenAlbum()
    {
        return await _albumRepository.GetMostListenAlbum();
    }
}