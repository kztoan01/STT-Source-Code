using core.Dtos.Album;
using core.Dtos.Artist;
using core.Dtos.User;
using core.Objects;

namespace service.Service.Interfaces;

public interface IAdminService
{
    Task<List<ArtistDTO>> getArtist(QueryArtist queryObject);
    Task<List<UserDTO>> getUser(QueryUser queryObject);
    Task<List<AlbumDTO>> GetAlbum(QueryAlbum queryObject);
    Task<bool> DeleteArtist(Guid artistId);
    Task<bool> DeletePlaylist(Guid playlistId);
    Task<bool> DeleteMusic(Guid musicId);
    Task DeleteAlbum(Guid albumId);
    Task DeleteUser(Guid userId);
}