using core.Models;
using data.Data;
using Microsoft.EntityFrameworkCore;
using repository.Repository.Interfaces;

namespace repository.Repository
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
            var playlist = await _context.Playlists.FindAsync(id);
            if (playlist == null)
                return null;

            _context.Playlists.Remove(playlist);
            await _context.SaveChangesAsync();
            return playlist;
        }

        public async Task<Playlist?> GetPlaylistByIdAsync(Guid id)
        {
            return await _context.Playlists.Include(p => p.playlistMusics).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Playlist?> UpdatePlaylistAsync(Guid id, Playlist playlist)
        {
            var existingPlaylist = await _context.Playlists
                .Include(p => p.playlistMusics)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (existingPlaylist == null)
                return null;

            existingPlaylist.playlistName = playlist.playlistName;
            existingPlaylist.playlistDescription = playlist.playlistDescription;
            existingPlaylist.playlistPicture = playlist.playlistPicture;
            existingPlaylist.createdDate = playlist.createdDate;
            existingPlaylist.updatedDate = playlist.updatedDate;
            existingPlaylist.userId = playlist.userId;

            foreach (var playlistMusic in playlist.playlistMusics)
            {
                var existingMusic = existingPlaylist.playlistMusics
                    .FirstOrDefault(pm => pm.musicId == playlistMusic.musicId);
                if (existingMusic != null)
                {
                    existingMusic.position = playlistMusic.position;
                }
                else
                {
                    existingPlaylist.playlistMusics.Add(playlistMusic);
                }
            }

            await _context.SaveChangesAsync();
            return existingPlaylist;
        }
        public async Task<List<Playlist>> GetUserPlaylistsAsync(string userId)
        {
            return await _context.Playlists
                .Where(p => p.userId == userId)
                .ToListAsync();
        }

        public async Task<List<Playlist>> GetPlaylistsByGenreNameAsync(string genreName)
        {
            return await _context.Playlists
                .Where(p => p.playlistMusics
                    .Any(pm => pm.Music.Genre.genreName == genreName))
                .ToListAsync();
        }

        public async Task<List<Playlist>> ShowPlaylistsByUserIdAsync(Guid userId)
        {
            return await _context.Playlists
                .Where(p => p.userId == userId.ToString())
                .ToListAsync();
        }
    }
}
