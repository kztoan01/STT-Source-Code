using core.Models;

namespace repository.Repository.Interfaces;

public interface IPlaylistRepository
{
    Task<Playlist?> GetPlaylistByIdAsync(Guid id);
    Task<Playlist> CreatePlaylistAsync(Playlist playlist);
    Task<Playlist?> UpdatePlaylistAsync(Guid id, Playlist playlist);
    Task<Playlist?> DeletePlaylistAsync(Guid id);
    Task<List<Playlist>> GetUserPlaylistsAsync(string userId);
    Task<List<Playlist>> GetPlaylistsByGenreNameAsync(string genreName);
    Task<List<Playlist>> ShowPlaylistsByUserIdAsync(Guid userId);
}