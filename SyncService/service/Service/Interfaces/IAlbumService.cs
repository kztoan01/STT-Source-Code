using core.Dtos.Album;
using core.Models;
using core.Objects;

namespace service.Service.Interfaces;

public interface IAlbumService
{
    Task<List<AlbumResponseDTO>> getAlbumByGenreNameAsync(string genreName);
    Task<List<AlbumResponseDTO>> getAllAlbumsAsync(QueryObject query);
    Task<AlbumResponseDTO> GetAlbumDetail(Guid albumId);
    Task<Album> GetMostListenAlbum();
    Task<Album> CreateAlbumAsync(CreateAlbumDTO albumDTO, Guid artistId);
    Task<bool> DeleteAlbumAsync(Guid albumId);
    Task<Album> GetAlbumByIdAsync(Guid albumId);
    Task<Album> EditAlbumAsync(CreateAlbumDTO albumDTO, Guid artistId, Guid albumId);

    Task<List<AlbumResponseDTO>> GetAllArtistAlbumsAsync(Guid artistId);
}