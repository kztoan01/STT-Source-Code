using core.Dtos.Album;
using core.Dtos.Artist;

namespace service.Service.Interfaces;

public interface IArtistService
{
    Task<List<MusicResponseDTO>> GetAllArtistMusicsAsync(Guid artistId);
    Task<List<AlbumResponseDTO>> GetAllArtistAlbumsAsync(Guid artistId);
    Task<ArtistDTO> GetArtistDTOById(Guid id);
}