using core.Dtos.Album;
using core.Models;

namespace repository.Repository.Interfaces;

public interface IAlbumRepository
{
    Task<List<AlbumResponseDTO>> getAlbumByGenreNameAsync(string genreName);
    Task<List<Album>> getAllAlbumsAsync();
    Task<List<AlbumDTO>> GetAlbumByContainArtistByArtistId(Guid artistId);
    Task<Album> GetMostListenAlbum();
    Task<AlbumResponseDTO> GetAlbumDetails(Guid albumId);
    Task<Album> CreateAlbumAsync(Album album);
    Task<bool> DeleteAlbumAsync(Guid albumId);
    Task<Album> GetAlbumById(Guid albumId);
    Task<Album> EditAlbumAsync(Album album);
    Task<List<AlbumResponseDTO>> GetAllArtistAlbumsAsync(Guid artistId);
}