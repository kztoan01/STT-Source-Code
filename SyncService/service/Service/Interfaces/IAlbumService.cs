using core.Dtos.Album;
using core.Models;

namespace service.Service.Interfaces;

public interface IAlbumService
{
    Task<List<Album>> getAlbumByGenreNameAsync(string genreName);
    Task<List<Album>> getAllAlbumsAsync();
    Task<AlbumResponseDTO> GetAlbumDetail(Guid albumId);
    Task<Album> GetMostListenAlbum();
    Task<Album> CreateAlbumAsync(CreateAlbumDTO albumDTO, Guid artistId);
    Task<Album> DeleteAlbumAsync(Guid albumId);
    Task<Album> GetAlbumByIdAsync(Guid albumId);
    Task<Album> EditAlbumAsync(CreateAlbumDTO albumDTO, Guid artistId, Guid albumId);
}