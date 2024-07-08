// using service.Services;

using core.Dtos.Album;
using core.Dtos.Artist;
using core.Models;

namespace repository.Repository.Interfaces;

public interface IArtistRepository
{
    Task<List<MusicResponseDTO>> GetAllArtistMusicsAsync(Guid artistId);
    Task<ArtistDTO> GetArtistDTOById(Guid id);
    Task<Artist> GetArtistByUserId(Guid userId);
    Task<Artist> CreateArtist(Artist artist);
}