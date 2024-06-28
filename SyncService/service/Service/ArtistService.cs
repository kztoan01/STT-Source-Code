using core.Dtos.Album;
using core.Dtos.Artist;
using core.Models;
using repository.Repository.Interfaces;
using service.Service.Interfaces;

namespace service.Service;

public class ArtistService : IArtistService
{
    private readonly IArtistRepository _artistRepository;

    public ArtistService(IArtistRepository artistRepository)
    {
        _artistRepository = artistRepository;
    }



    public async Task<ArtistDTO> GetArtistDTOById(Guid id)
    {
        return await _artistRepository.GetArtistDTOById(id);
    }

    public async Task<List<MusicResponseDTO>> GetAllArtistMusicsAsync(Guid artistId)
    {
        return await _artistRepository.GetAllArtistMusicsAsync(artistId);
    }

    public async Task<Artist> GetArtistByUserIdAsync(Guid userId)
    {
        return await _artistRepository.GetArtistByUserId(userId);
    }
}