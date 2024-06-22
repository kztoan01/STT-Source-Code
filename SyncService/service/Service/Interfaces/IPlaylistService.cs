using core.Dtos.Album;
using core.Dtos.Playlist;
using core.Models;

namespace service.Service.Interfaces
{
    public interface IPlaylistService
    {
        Task<Playlist?> GetPlaylistByIdAsync(Guid id);
        Task<Playlist> CreatePlaylistAsync(Playlist playlist);
        Task<Playlist?> UpdatePlaylistAsync(Guid id, Playlist playlistModel);
        Task<Playlist?> DeletePlaylistAsync(Guid id);
        Task<List<PlaylistDTO>> GetUserPlaylistsAsync(string userId);
        Task<List<Playlist>> GetPlaylistsByGenreNameAsync(string genreName);
        Task<List<Playlist>> ShowPlaylistsByUserIdAsync(Guid userId);
        Task<string> AddMusicIntoPlaylistAsync(Guid musicId, Guid playlistId);
        Task<string> AddEntireAlbumIntoPlaylistAsync(Guid albumId, Guid playlistId);
        Task<List<AlbumDTO>> GetAlbumByContainArtistByArtistId(Guid artistId);
        Task<string> DeleteMusicInPlaylistAsync(Guid musicId, Guid playlistId);
        Task<string> ChangeMusicPositionInPlaylistAsync(Guid musicId, int newPosition, Guid playlistId);
    }
}
