using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sync_service.Dtos.Playlist;
using sync_service.Interfaces;
using sync_service.Models;
using sync_service.Service.Interfaces;

namespace sync_service.Service
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IPlaylistRepository _playlistRepository;

        public PlaylistService(IPlaylistRepository playlistRepository)
        {
            _playlistRepository = playlistRepository;
        }
        public async Task<Playlist> CreatePlaylistAsync(Playlist playlist)
        {
            return await _playlistRepository.CreatePlaylistAsync(playlist);
        }

        public async Task<Playlist?> DeletePlaylistAsync(Guid id)
        {
            return await _playlistRepository.DeletePlaylistAsync(id);
        }

        public async Task<Playlist?> GetPlaylistByIdAsync(Guid id)
        {
            return await _playlistRepository.GetPlaylistByIdAsync(id);
        }

        public async Task<List<PlaylistDTO>> GetUserPlaylistAsync(string userId)
        {
            return await _playlistRepository.GetUserPlaylistAsync(userId);
        }

        public async Task<Playlist?> UpdatePlaylistAsync(Guid id, Playlist playlistModel)
        {
            return await _playlistRepository.UpdatePlaylistAsync(id, playlistModel);
        }
    }
}