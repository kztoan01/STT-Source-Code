using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sync_service.Dtos.Album;
using sync_service.Dtos.Playlist;
using sync_service.Models;

namespace sync_service.Service.Interfaces
{
    public interface IPlaylistService
    {
        Task<List<PlaylistDTO>> GetUserPlaylistAsync(string userId);
        Task<List<Playlist>> GetPlaylistsByGenreNameAsync(string genreName);

        Task<Playlist?> GetPlaylistByIdAsync(Guid id);
        Task<Playlist> CreatePlaylistAsync(Playlist playlist);
        Task<Playlist?> UpdatePlaylistAsync(Guid id, Playlist playlistModel);
        Task<Playlist?> DeletePlaylistAsync(Guid id);
        Task<List<Playlist>> ShowPlaylistByUserId(Guid UserId);

        Task<string> AddMusicIntoPlaylist(Guid musicId, Guid playlistId);

        Task<string> AddEntireAlbumIntoPlaylist(Guid albumId, Guid playlistId);

        Task<List<AlbumDTO>> GetAlbumByContainArtistByArtistId(Guid artistId);
        Task<string> DeleteAMusicInPlaylist(Guid musicId, Guid playlistId);

        Task<string> ChangeMusicPositionInPlaylist(Guid musicId1, Guid musicId2, Guid playlistId);
    }
}