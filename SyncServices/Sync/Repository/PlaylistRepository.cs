using Sync.Model;
using Sync.DTOs;
using System;
using System.Linq;

namespace Sync.Repository
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly EFDataContext _context;

        public PlaylistRepository(EFDataContext context)
        {
            _context = context;
        }

        public PlaylistDTO AddPlaylist(PlaylistDTO playlistDTO)
        {
            var newPlaylist = new Playlist
            {
                Id = Guid.NewGuid(),
                PlaylistName = playlistDTO.PlaylistName,
                PlaylistDescription = playlistDTO.PlaylistDescription,
                PlaylistPicture = playlistDTO.PlaylistPicture,
                CreationDay = DateTime.UtcNow
            };

            _context.Playlists.Add(newPlaylist);
            _context.SaveChanges();

            return new PlaylistDTO
            {
                Id = newPlaylist.Id,
                PlaylistName = newPlaylist.PlaylistName,
                PlaylistDescription = newPlaylist.PlaylistDescription,
                PlaylistPicture = newPlaylist.PlaylistPicture,
                CreationDay = newPlaylist.CreationDay
            };
        }

        public PlaylistDTO UpdatePlaylist(PlaylistDTO playlistDTO)
        {
            var playlist = _context.Playlists.FirstOrDefault(p => p.Id == playlistDTO.Id);
            if (playlist == null)
            {
                throw new InvalidOperationException("Playlist not found.");
            }

            playlist.PlaylistName = playlistDTO.PlaylistName;
            playlist.PlaylistDescription = playlistDTO.PlaylistDescription;
            playlist.PlaylistPicture = playlistDTO.PlaylistPicture;

            _context.Playlists.Update(playlist);
            _context.SaveChanges();

            return new PlaylistDTO
            {
                Id = playlist.Id,
                PlaylistName = playlist.PlaylistName,
                PlaylistDescription = playlist.PlaylistDescription,
                PlaylistPicture = playlist.PlaylistPicture,
                CreationDay = playlist.CreationDay
            };
        }

        public void DeletePlaylist(Guid playlistId)
        {
            var playlist = _context.Playlists.FirstOrDefault(p => p.Id == playlistId);
            if (playlist == null)
            {
                throw new InvalidOperationException("Playlist not found.");
            }

            _context.Playlists.Remove(playlist);
            _context.SaveChanges();
        }

        public PlaylistDTO GetPlaylistById(Guid playlistId)
        {
            var playlist = _context.Playlists.FirstOrDefault(p => p.Id == playlistId);
            if (playlist == null)
            {
                return null;
            }

            return new PlaylistDTO
            {
                Id = playlist.Id,
                PlaylistName = playlist.PlaylistName,
                PlaylistDescription = playlist.PlaylistDescription,
                PlaylistPicture = playlist.PlaylistPicture,
                CreationDay = playlist.CreationDay
            };
        }

        public List<PlaylistDTO> GetAllPlaylists()
        {
            return _context.Playlists.Select(playlist => new PlaylistDTO
            {
                Id = playlist.Id,
                PlaylistName = playlist.PlaylistName,
                PlaylistDescription = playlist.PlaylistDescription,
                PlaylistPicture = playlist.PlaylistPicture,
                CreationDay = playlist.CreationDay
            }).ToList();
        }

        public int GetTotalPlaylists()
        {
            return _context.Playlists.Count();
        }

        public int GetPlaylistsCreatedInLastMonth()
        {
            var lastMonth = DateTime.UtcNow.AddMonths(-1);
            return _context.Playlists.Count(p => p.CreationDay >= lastMonth);
        }

        public PlaylistDTO GetMostPopularPlaylist()
        {
            var mostPopularPlaylist = _context.Playlists
                .OrderByDescending(p => p.UserPlaylists.Count)
                .FirstOrDefault();

            if (mostPopularPlaylist == null)
            {
                return null;
            }

            return new PlaylistDTO
            {
                Id = mostPopularPlaylist.Id,
                PlaylistName = mostPopularPlaylist.PlaylistName,
                PlaylistDescription = mostPopularPlaylist.PlaylistDescription,
                PlaylistPicture = mostPopularPlaylist.PlaylistPicture,
                CreationDay = mostPopularPlaylist.CreationDay
            };
        }

        public List<UserPlaylistDTO> GetUserPlaylistsByUserId(Guid userId)
        {
            return _context.UserPlaylists
                .Where(up => up.UserId == userId)
                .Select(up => new UserPlaylistDTO
                {
                    Id = up.Id,
                    UserId = up.UserId,
                    PlaylistId = up.PlaylistId,
                    CreatedDate = up.CreatedDate,
                    PlaylistName = up.Playlist.PlaylistName
                }).ToList();
        }
    }
}
