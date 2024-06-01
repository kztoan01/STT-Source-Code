using Sync.DTOs;
using Sync.Model;
using Sync.Repository;

namespace Sync.Services.iml
{
    public class PlayListService : IPlayListService
    {
        private IPlaylistRepository playlistRepository;
        private readonly EFDataContext _context;
        public PlayListService(EFDataContext context) {
            _context = context;
            playlistRepository = new PlaylistRepository(context);
        }
        public PlaylistDTO AddPlaylist(PlaylistDTO playlistDTO)
        {
          return  playlistRepository.AddPlaylist(playlistDTO);
        }

        public void DeletePlaylist(Guid playlistId)
        {
            playlistRepository.DeletePlaylist(playlistId);
        }

        public List<PlaylistDTO> GetAllPlaylists()
        {
            return playlistRepository.GetAllPlaylists();
        }

        public PlaylistDTO GetMostPopularPlaylist()
        {
           return playlistRepository.GetMostPopularPlaylist();
        }

        public PlaylistDTO GetPlaylistById(Guid playlistId)
        {
           return playlistRepository.GetPlaylistById(playlistId);
        }

        public int GetPlaylistsCreatedInLastMonth()
        {
            return playlistRepository.GetPlaylistsCreatedInLastMonth();
        }

        public int GetTotalPlaylists()
        {
           return playlistRepository.GetTotalPlaylists();
        }

        public List<UserPlaylistDTO> GetUserPlaylistsByUserId(Guid userId)
        {
            return playlistRepository.GetUserPlaylistsByUserId(userId);
        }

        public PlaylistDTO UpdatePlaylist(PlaylistDTO playlistDTO)
        {
            return playlistRepository.UpdatePlaylist(playlistDTO);
        }
    }
}
