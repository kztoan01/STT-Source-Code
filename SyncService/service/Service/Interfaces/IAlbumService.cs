using core.Dtos.Album;
using core.Models;

namespace service.Service.Interfaces;

public interface IAlbumService
{
    Task<List<Album>> getAlbumByGenreNameAsync(string genreName);
    Task<List<Album>> getAllAlbumsAsync();
    Task<AlbumResponseDTO> GetAlbumDetail(Guid albumId);
    Task<Album> GetMostListenAlbum();
}