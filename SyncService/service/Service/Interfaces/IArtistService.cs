using core.Dtos.Artist;
using core.Models;

namespace service.Service.Interfaces;

public interface IArtistService
{
    Task<List<MusicResponseDTO>> GetAllArtistMusicsAsync(Guid artistId);
    Task<bool> UpdateArtistInforAsync(Guid userId, ArtistImageDTO artistImage);
    Task<ArtistDTO> GetArtistDTOById(Guid id);
    Task<Artist> GetArtistByUserIdAsync(Guid userId);
    Task<Artist> CreateArtist(Artist artist);
}