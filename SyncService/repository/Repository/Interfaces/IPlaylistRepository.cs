using core.Dtos.Playlist;
using core.Models;

namespace repository.Repository.Interfaces;

public interface IPlaylistRepository
{
    Task<List<PlaylistDTO>> GetUserPlaylistAsync(string userId);
    Task<List<Playlist>> GetPlaylistsByGenreNameAsync(string genreName);
    Task<Playlist?> GetPlaylistByIdAsync(Guid id);
    Task<Playlist> CreatePlaylistAsync(Playlist playlist);
    Task<Playlist?> UpdatePlaylistAsync(Guid id, Playlist playlistModel);
    Task<Playlist?> DeletePlaylistAsync(Guid id);
    Task<List<Playlist>> ShowPlaylistByUserId(Guid userId);
    Task<string> AddMusicIntoPlaylist(Guid musicId, Guid playlistId);
    Task<string> AddEntireAlbumIntoPlaylist(Guid albumId, Guid playlistId);
    Task<string> DeleteAMusicInPlaylist(Guid musicId, Guid playlistId);
    Task<string> ChangeMusicPositionInPlaylist(Guid musicId1, int newPosition, Guid playlistId);
}