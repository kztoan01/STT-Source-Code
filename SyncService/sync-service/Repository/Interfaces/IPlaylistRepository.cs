using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sync_service.Dtos.Playlist;
using sync_service.Models;

namespace sync_service.Interfaces
{
    public interface IPlaylistRepository
    {
        Task<List<PlaylistDTO>> GetUserPlaylistAsync(string userId);
        Task<Playlist?> GetPlaylistByIdAsync(Guid id);
        Task<Playlist> CreatePlaylistAsync(Playlist playlist);
        Task<Playlist?> UpdatePlaylistAsync(Guid id, Playlist playlistModel);
        Task<Playlist?> DeletePlaylistAsync(Guid id);
    }
}