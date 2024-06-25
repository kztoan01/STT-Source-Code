using core.Models;
using data.Data;
using Microsoft.EntityFrameworkCore;
using repository.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace repository.Repository
{
    public class MusicRepository : IMusicRepository
    {
        private readonly ApplicationDBContext _context;

        public MusicRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Music> CreateMusicAsync(Music music)
        {
            await _context.Musics.AddAsync(music);
            await _context.SaveChangesAsync();
            return music;
        }

        public async Task<Music?> GetMusicByIdAsync(Guid id)
        {
            return await _context.Musics
                .Include(m => m.Album)
                .Include(m => m.Artist)
                    .ThenInclude(a => a.User)
                .Include(m => m.Genre)
                .Include(m => m.MusicListens)
                .FirstOrDefaultAsync(m => m.Id == id);
        }


        public async Task<List<Music>> GetAllMusicAsync()
        {
            return await _context.Musics
                .Include(m => m.Album)
                .Include(m => m.Artist)
                .Include(m => m.Genre)
                .Include(m => m.Artist).ThenInclude(a => a.User)
                .Include(m => m.MusicListens)
                .ToListAsync();

           
        }

        public async Task<Music?> UpdateMusicAsync(Guid id, Music music)
        {
            var existingMusic = await _context.Musics.FindAsync(id);
            if (existingMusic == null)
                return null;

            existingMusic.musicTitle = music.musicTitle;
            existingMusic.musicUrl = music.musicUrl;
            existingMusic.musicPicture = music.musicPicture;
            existingMusic.musicPlays = music.musicPlays;
            existingMusic.musicDuration = music.musicDuration;
            existingMusic.releaseDate = music.releaseDate;
            existingMusic.albumId = music.albumId;
            existingMusic.artistId = music.artistId;
            existingMusic.genreId = music.genreId;

            await _context.SaveChangesAsync();
            return existingMusic;
        }

        public async Task<bool> DeleteMusicAsync(Guid id)
        {
            var music = await _context.Musics.FindAsync(id);
            if (music == null)
                return false;

            _context.Musics.Remove(music);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetListenCountAsync(Guid musicId, DateTime startDate, DateTime endDate)
        {
            return await _context.MusicListens
                .Where(ml => ml.MusicId == musicId && ml.ListenDate >= startDate && ml.ListenDate <= endDate)
                .SumAsync(ml => (int?)ml.ListenCount) ?? 0;
        }

        public async Task<List<Music>>  GetMusicByAlbumIdAsync(Guid albumId)
        {
            return await _context.Musics.Where(x => x.albumId.Equals(albumId)).ToListAsync();
        }

        public async Task<List<PlaylistMusic>> GetMusicInPlaylistByPlaylsitIdAsync(Guid playlistId)
        {
            return await _context.PlaylistMusics.Where(x => x.playlistId.Equals(playlistId)).ToListAsync();
        }

    }
}
