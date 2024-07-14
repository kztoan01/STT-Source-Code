using core.Dtos.Album;
using core.Dtos.Artist;
using core.Models;
using Microsoft.AspNetCore.Http;

namespace service.Service.Interfaces;

public interface IArtistService
{
    Task<List<MusicResponseDTO>> GetAllArtistMusicsAsync(Guid artistId);
    Task<bool> UpdateArtistImageAsync(ArtistImageDTO artistImage);
    Task<ArtistDTO> GetArtistDTOById(Guid id);
    Task<Artist> GetArtistByUserIdAsync(Guid userId);
    Task<Artist> CreateArtist(Artist artist);
}