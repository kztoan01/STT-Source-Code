using core.Dtos.Artist;
using core.Models;

namespace service.Service.Interfaces;

public interface IAdminService
{
    Task<List<ArtistDTO>> getAllArtist(int sortType);
    Task<List<User>> getAllUser(int sortType);
    Task<List<Album>> getAllAlbum(int sortType);
    // Task deleteArtist()
}