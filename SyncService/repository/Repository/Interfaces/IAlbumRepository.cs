using core.Dtos.Album;
using core.Models;

namespace repository.Repository.Interfaces;

public interface IAlbumRepository
{
    Task<List<Album>> getAlbumByGenreNameAsync(string genreName);
    Task<List<Album>> getAllAlbumsAsync();
    Task<List<AlbumDTO>> GetAlbumByContainArtistByArtistId(Guid artistId);
    Task<Album> GetMostListenAlbum();
    Task<AlbumResponseDTO> GetAlbumDetails(Guid albumId);
}