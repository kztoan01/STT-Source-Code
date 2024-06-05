using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sync_service.Models;

namespace sync_service.Interfaces
{
    public interface IPlaylistRepository
    {
        Task<List<Playlist>> GetUserPlaylistAsync(int userId);
        Task<Playlist?> GetPlaylistByIdAsync(int id);
        Task<Playlist> CreatePlaylistAsync(Playlist playlist);
        Task<Playlist?> UpdatePlaylistAsync(int id, Playlist playlistModel);
        Task<Playlist?> DeletePlaylistAsync(int id);
    }
}