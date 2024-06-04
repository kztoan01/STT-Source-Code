using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sync_music_service.Dtos.Playlist;
using sync_music_service.Models;

namespace sync_music_service.Mappers
{
    public static class PlaylistMapper
    {
        public static PlaylistDTO ToPlaylistDTO(this Playlist playlist)
        {
            return new PlaylistDTO
            {
                Id = playlist.Id,
                playlistName = playlist.playlistName,
                playlistDescription = playlist.playlistDescription,
                playlistPicture = playlist.playlistPicture,
                createdDate = playlist.createdDate,
                updatedDate = playlist.updatedDate,
                userId = playlist.userId,
            };
        }

        public static Playlist ToPlaylistFromCreate (this CreatePlaylistDTO playlistDTO)
        {
            return new Playlist
            {
                playlistName = playlistDTO.playlistName,
                playlistDescription = playlistDTO.playlistDescription,
                playlistPicture = playlistDTO.playlistPicture,
            };
        }
    }
}