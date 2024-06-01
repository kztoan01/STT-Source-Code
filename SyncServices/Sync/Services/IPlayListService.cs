using Sync.DTOs;

namespace Sync.Services
{
    public interface iPlayListService
    {
        PlaylistDTO AddPlaylist(PlaylistDTO playlistDTO);
        PlaylistDTO UpdatePlaylist(PlaylistDTO playlistDTO);
        void DeletePlaylist(Guid playlistId);
        PlaylistDTO GetPlaylistById(Guid playlistId);
        List<PlaylistDTO> GetAllPlaylists();

        int GetTotalPlaylists();
        int GetPlaylistsCreatedInLastMonth();
        PlaylistDTO GetMostPopularPlaylist();
        List<UserPlaylistDTO> GetUserPlaylistsByUserId(Guid userId);
    }
}
