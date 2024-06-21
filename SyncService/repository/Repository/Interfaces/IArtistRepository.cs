// using service.Services;

using core.Dtos.Album;
using core.Dtos.Artist;

namespace repository.Repository.Interfaces;

public interface IArtistRepository
{
    Task<List<MusicResponseDTO>> GetAllArtistMusicsAsync(Guid artistId);
    Task<List<AlbumResponseDTO>> GetAllArtistAlbumsAsync(Guid artistId);
    Task<ArtistDTO> GetArtistDTOById(Guid id);
}