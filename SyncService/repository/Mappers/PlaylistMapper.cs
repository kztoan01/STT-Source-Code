using core.Dtos.Playlist;
using core.Models;

namespace service.Service.Mappers;

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
            userId = playlist.userId
        };
    }

    public static Playlist ToPlaylistFromCreate(this CreatePlaylistDTO playlistDTO, string userId)
    {
        return new Playlist
        {
            playlistName = playlistDTO.playlistName,
            playlistDescription = playlistDTO.playlistDescription,
            playlistPicture = playlistDTO.playlistPicture,
            userId = userId
        };
    }
}