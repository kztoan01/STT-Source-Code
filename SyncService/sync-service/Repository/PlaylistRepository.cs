using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using sync_service.Data;
using sync_service.Dtos.Playlist;
using sync_service.Interfaces;
using sync_service.Models;

namespace sync_service.Repository
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly ApplicationDBContext _context;

        public PlaylistRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Playlist> CreatePlaylistAsync(Playlist playlist)
        {
            await _context.Playlists.AddAsync(playlist);
            await _context.SaveChangesAsync();
            return playlist;
        }

        public async Task<Playlist?> DeletePlaylistAsync(Guid id)
        {
            var playlistModel = await _context.Playlists.FirstOrDefaultAsync(x => x.Id == id);

            if(playlistModel == null)
                return null;

            _context.Playlists.Remove(playlistModel);
            await _context.SaveChangesAsync();

            return playlistModel;
        }

        public async Task<Playlist?> GetPlaylistByIdAsync(Guid id)
        {
            return await _context.Playlists.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Playlist>> GetPlaylistsByGenreNameAsync(string genreName)
        {
            // Playlist -> PlaylistMusic -> Music -> Genre
            // handle mapper later 
            return await _context.Playlists
                .Where(p=> p.playlistMusics
                .Any(pm => pm.Music.Genre.genreName == genreName))
                .Include(pm => pm.playlistMusics)
                .ToListAsync();
        }

        public async Task<List<PlaylistDTO>> GetUserPlaylistAsync(string userId)
        {
            return await _context.Playlists
                .Where(c => c.userId == userId)
                .Select(c => new PlaylistDTO
                {
                    Id = c.Id,
                    playlistName = c.playlistName,
                    playlistDescription = c.playlistDescription,
                    createdDate = c.createdDate,
                    updatedDate = c.updatedDate,
                })
                .ToListAsync();
        }

        public async Task<Playlist?> UpdatePlaylistAsync(Guid id, Playlist playlistModel)
        {
            var existingPlaylist = await _context.Playlists.FindAsync(id);

            if(existingPlaylist == null)
                return null;

            existingPlaylist.playlistName = playlistModel.playlistName;
            existingPlaylist.playlistDescription = playlistModel.playlistDescription;
            existingPlaylist.playlistPicture = playlistModel.playlistPicture;
            existingPlaylist.createdDate = playlistModel.createdDate;
            existingPlaylist.updatedDate = playlistModel.updatedDate;
            existingPlaylist.userId = existingPlaylist.userId;

            await _context.SaveChangesAsync();

            return existingPlaylist;
        }
    }
}