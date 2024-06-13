using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using sync_service.Data;
using sync_service.Dtos.Music;
using sync_service.Dtos.Playlist;
using sync_service.Interfaces;
using sync_service.Mappers;
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

        public async Task<string> AddMusicIntoPlaylist(Guid musicId, Guid playlistId)
        {

            var playlist = await _context.Playlists.Include(p => p.playlistMusics).FirstOrDefaultAsync(p => p.Id == playlistId);
            var music = await _context.Musics.FirstOrDefaultAsync(m => m.Id == musicId);


            if (playlist == null || music == null)
            {
                return "Music or Playlist are not available!";
            }

            if (playlist.playlistMusics.Any(pm => pm.musicId == musicId))
            {
                return "This music is already added into this Playlist.";
            }

            var nextPosition = playlist.playlistMusics.Count + 1;

            var playlistMusic = new PlaylistMusic
            {
                playlistId = playlistId,
                musicId = musicId,
                addedAt = DateTime.UtcNow,
                position = nextPosition,
                Music = music,
                Playlist = playlist,
            };

            _context.PlaylistMusics.Add(playlistMusic);

            await _context.SaveChangesAsync();

            return "Added successfully!";
        }
        public async Task<List<MusicDTO>> GetMusicByAlbumId(Guid albumId)
        {
            List<Music> musics = await _context.Musics
                .Include(m => m.Album)
                .Include(m => m.Artist)
                .ThenInclude(a => a.User)
                .Include(m => m.Genre)
                .Where(m => m.albumId.Equals(albumId))
                .ToListAsync();

            if (musics == null || musics.Count == 0)
            {
                return null; 
            }

            var musicDTOs = musics.Select(music => MusicMapper.Convert(music)).ToList();

            return musicDTOs;
        }

        public async Task<string> AddEntireAlbumIntoPlaylist(Guid albumId, Guid playlistId)
        {
            List<MusicDTO> musicDTOs = await GetMusicByAlbumId(albumId);

            if (musicDTOs == null || musicDTOs.Count == 0)
            {
                return "No music found in the specified album.";
            }

            var playlist = await _context.Playlists.Include(p => p.playlistMusics).FirstOrDefaultAsync(p => p.Id == playlistId);

            if (playlist == null)
            {
                return "Playlist not found.";
            }

            List<string> addedMusicTitles = new List<string>();
            List<string> skippedMusicTitles = new List<string>();

            foreach (var music in musicDTOs)
            {
                var result = await AddMusicIntoPlaylist(music.Id, playlistId);

                if (result.Equals("This music is already added into this Playlist."))
                {
                    skippedMusicTitles.Add(music.musicTitle);
                }
                else
                {
                    addedMusicTitles.Add(music.musicTitle);
                }
            }

            string message = "Album added to playlist successfully!";

            if (addedMusicTitles.Count > 0)
            {
                message += "Added music:" + string.Join(";", addedMusicTitles);
            }

            if (skippedMusicTitles.Count > 0)
            {
                message += "Skipped music" + string.Join(";", skippedMusicTitles);
            }

            return message;
        }

        public async Task<List<Playlist>> ShowPlaylistByUserId(Guid UserId)
        {
            return await _context.Playlists
                .Where(p => p.userId == UserId.ToString())
                .ToListAsync();
        }
        public async Task<string> DeleteAMusicInPlaylist(Guid musicId, Guid playlistId)
        {
            var playlist = await _context.Playlists
                .Include(p => p.playlistMusics)
                .FirstOrDefaultAsync(p => p.Id == playlistId);

 
            if (playlist == null)
            {
                return "Plau list is null!"; 
            }

            var music = await _context.Musics.FindAsync(musicId);

            if (music == null)
            {
                return "Music list is null!";
            }

            var playlistMusic = playlist.playlistMusics.FirstOrDefault(pm => pm.musicId == musicId);

            if (playlistMusic == null)
            {
                return "Invalid request";
            }

            playlist.playlistMusics.Remove(playlistMusic);

            await _context.SaveChangesAsync();
            return "Removed music from playlist successfully!";
        }

        public async Task<string> ChangeMusicPositionInPlaylist(Guid musicId1, Guid musicId2, Guid playlistId)
        {
            var music1 = await _context.PlaylistMusics.FirstOrDefaultAsync(m => m.musicId == musicId1 && m.playlistId == playlistId);
            var music2 = await _context.PlaylistMusics.FirstOrDefaultAsync(m => m.musicId == musicId2 && m.playlistId == playlistId);

            if (music1 == null || music2 == null)
            {
                return "One or both of the specified music IDs were not found in the playlist.";
            }

            List<PlaylistMusic> playlistMusics = _context.PlaylistMusics
                .Where(pm => pm.playlistId == playlistId)
                .OrderBy(pm => pm.position)
                .ToList();



            if(music1.position > music2.position)
            {
                int oldPosition1 = music1.position;
                music1.position = music2.position;
                foreach (var playlistMusic in playlistMusics)
                {
                        if (playlistMusic.position >= music2.position && playlistMusic.position < oldPosition1)
                        {
                            playlistMusic.position = playlistMusic.position + 1;
                        }                
                }

            }
            else
            {

               int oldPosition1 = music1.position;
               int music2Position = music2.position;
                foreach (var playlistMusic in playlistMusics)
                {
                    if (playlistMusic.position <= music2.position && playlistMusic.position > oldPosition1)
                    {
                        playlistMusic.position = playlistMusic.position - 1;
                    }
                }
                music1.position = music2Position;
            }




            await _context.SaveChangesAsync();

            return "Music positions in the playlist have been updated successfully.";
        }


    }
}