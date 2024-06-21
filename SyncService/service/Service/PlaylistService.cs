using core.Dtos.Playlist;
using core.Models;
using repository.Repository.Interfaces;
using service.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using core.Dtos.Album;

namespace service.Service
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IPlaylistRepository _playlistRepository;
        private readonly IMusicRepository _musicRepository; 
        private readonly IAlbumRepository _albumRepository;

        public PlaylistService(IPlaylistRepository playlistRepository, IMusicRepository musicRepository, IAlbumRepository albumRepository)
        {
            _playlistRepository = playlistRepository;
            _musicRepository = musicRepository;
            _albumRepository = albumRepository;
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

        public async Task<List<PlaylistDTO>> GetUserPlaylistsAsync(string userId)
        {
            var playlists = await _playlistRepository.GetUserPlaylistsAsync(userId);
            return playlists.Select(p => new PlaylistDTO
            {
                Id = p.Id,
                playlistName = p.playlistName,
                playlistDescription = p.playlistDescription,
                createdDate = p.createdDate,
                updatedDate = p.updatedDate
            }).ToList();
        }

        public async Task<List<Playlist>> GetPlaylistsByGenreNameAsync(string genreName)
        {
            return await _playlistRepository.GetPlaylistsByGenreNameAsync(genreName);
        }

        public async Task<List<Playlist>> ShowPlaylistsByUserIdAsync(Guid userId)
        {
            return await _playlistRepository.ShowPlaylistsByUserIdAsync(userId);
        }

        public async Task<Playlist?> UpdatePlaylistAsync(Guid id, Playlist playlistModel)
        {
            return await _playlistRepository.UpdatePlaylistAsync(id, playlistModel);
        }

        public async Task<string> AddMusicIntoPlaylistAsync(Guid musicId, Guid playlistId)
        {
            var playlist = await _playlistRepository.GetPlaylistByIdAsync(playlistId);
            var music = await _musicRepository.GetMusicByIdAsync(musicId);

            if (playlist == null || music == null)
                return "Music or Playlist are not available!";

            if (playlist.playlistMusics.Any(pm => pm.musicId == musicId))
                return "This music is already added into this Playlist.";

            var nextPosition = playlist.playlistMusics.Count + 1;

            var playlistMusic = new PlaylistMusic
            {
                playlistId = playlistId,
                musicId = musicId,
                addedAt = DateTime.UtcNow,
                position = nextPosition,
                Music = music,
                Playlist = playlist
            };

            playlist.playlistMusics.Add(playlistMusic);
            await _playlistRepository.UpdatePlaylistAsync(playlistId, playlist);

            return "Added successfully!";
        }

        public async Task<string> AddEntireAlbumIntoPlaylistAsync(Guid albumId, Guid playlistId)
        {
            var musics = await _musicRepository.GetMusicByAlbumIdAsync(albumId);

            if (musics == null || musics.Count == 0)
                return "No music found in the specified album.";

            var playlist = await _playlistRepository.GetPlaylistByIdAsync(playlistId);

            if (playlist == null)
                return "Playlist not found.";

            var addedMusicTitles = new List<string>();
            var skippedMusicTitles = new List<string>();

            foreach (var music in musics)
            {
                var result = await AddMusicIntoPlaylistAsync(music.Id, playlistId);

                if (result.Equals("This music is already added into this Playlist."))
                    skippedMusicTitles.Add(music.musicTitle);
                else
                    addedMusicTitles.Add(music.musicTitle);
            }

            var message = "Album added to playlist successfully!";

            if (addedMusicTitles.Count > 0)
                message += " Added music: " + string.Join(";", addedMusicTitles);

            if (skippedMusicTitles.Count > 0)
                message += " Skipped music: " + string.Join(";", skippedMusicTitles);

            return message;
        }

        public async Task<List<AlbumDTO>> GetAlbumByContainArtistByArtistId(Guid artistId)
        {
            return await _albumRepository.GetAlbumByContainArtistByArtistId(artistId);
        }

        public async Task<string> DeleteMusicInPlaylistAsync(Guid musicId, Guid playlistId)
        {
            var playlist = await _playlistRepository.GetPlaylistByIdAsync(playlistId);

            if (playlist == null) return "Playlist is null!";

            var music = await _musicRepository.GetMusicByIdAsync(musicId);

            if (music == null) return "Music is null!";

            var playlistMusic = playlist.playlistMusics.FirstOrDefault(pm => pm.musicId == musicId);

            if (playlistMusic == null) return "Invalid";

            var deletedPosition = playlistMusic.position;

            playlist.playlistMusics.Remove(playlistMusic);

            foreach (var item in playlist.playlistMusics.Where(pm => pm.position > deletedPosition))
                item.position--;

            await _playlistRepository.UpdatePlaylistAsync(playlistId, playlist);
            return "Removed music from playlist successfully!";
        }


        public async Task<string> ChangeMusicPositionInPlaylistAsync(Guid musicId, int newPosition, Guid playlistId)
        {
            var playlist = await _playlistRepository.GetPlaylistByIdAsync(playlistId);

            if (playlist == null) return "Playlist not found!";

            var music = playlist.playlistMusics.FirstOrDefault(pm => pm.musicId.Equals(musicId));

            if (music == null) return "Music not found in the playlist.";

            var oldPosition = music.position;
            if (oldPosition == newPosition) return "New position is the same as old position.";

            var playlistMusics = playlist.playlistMusics.OrderBy(pm => pm.position).ToList();

            if (oldPosition > newPosition)
            {
                
                var changeList = playlistMusics
                    .Where(pm => pm.position >= newPosition && pm.position < oldPosition)
                    .ToList();
                foreach (var item in changeList) item.position++;
                music.position = newPosition;
            }
            else if (oldPosition < newPosition)
            {
                var changeList = playlistMusics
                    .Where(pm => pm.position > oldPosition && pm.position <= newPosition)
                    .ToList();
                foreach (var item in changeList) item.position--;
                music.position = newPosition;
            }

            await _playlistRepository.UpdatePlaylistAsync(playlistId, playlist);
            return "Music positions in the playlist have been updated successfully.";
        }


    }
}
