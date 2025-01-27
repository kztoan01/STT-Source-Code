// using service.Services;

using core.Dtos.Artist;
using core.Models;

namespace repository.Repository.Interfaces;

public interface IArtistRepository
{
    Task<List<MusicResponseDTO>> GetAllArtistMusicsAsync(Guid artistId);
    Task<ArtistDTO> GetArtistDTOById(Guid id);
    Task<Artist> GetArtistByUserId(Guid userId);
    Task<Artist> CreateArtist(Artist artist);
    Task<bool> updateArtist(Artist artist);
    Task<List<ArtistDTO>> GetAllArtistDTOs();
    Task<bool> DeleteArtist(Guid id);
}