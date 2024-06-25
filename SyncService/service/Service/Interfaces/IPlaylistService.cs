using core.Dtos.Album;
using core.Dtos.Playlist;
using core.Models;
using core.Objects;

namespace service.Service.Interfaces
{
    public interface IPlaylistService
    {
        Task<PlaylistDTO> GetPlaylistByIdAsync(Guid id);
        Task<Playlist> CreatePlaylistAsync(Playlist playlist);
        Task<Playlist?> UpdatePlaylistAsync(Guid id, Playlist playlistModel);
        Task<Playlist?> DeletePlaylistAsync(Guid id);
        Task<List<PlaylistDTO>> GetUserPlaylistsAsync(string userId, QueryObject query);
        Task<List<PlaylistDTO>> GetPlaylistsByGenreNameAsync(string genreName, QueryObject query);
        Task<List<PlaylistDTO>> ShowPlaylistsByUserIdAsync(Guid userId, QueryObject query);
        Task<string> AddMusicIntoPlaylistAsync(Guid musicId, Guid playlistId);
        Task<string> AddEntireAlbumIntoPlaylistAsync(Guid albumId, Guid playlistId);
        Task<List<AlbumDTO>> GetAlbumByContainArtistByArtistId(Guid artistId);
        Task<string> DeleteMusicInPlaylistAsync(Guid musicId, Guid playlistId);
        Task<string> ChangeMusicPositionInPlaylistAsync(Guid musicId, int newPosition, Guid playlistId);
    }
}
