using core.Dtos.Album;
using core.Dtos.Artist;
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

    public async Task<List<AlbumResponseDTO>> GetAllArtistAlbumsAsync(Guid artistId)
    {
        return await _artistRepository.GetAllArtistAlbumsAsync(artistId);
    }

    public async Task<ArtistDTO> GetArtistDTOById(Guid id)
    {
        return await _artistRepository.GetArtistDTOById(id);
    }

    public async Task<List<MusicResponseDTO>> GetAllArtistMusicsAsync(Guid artistId)
    {
        return await _artistRepository.GetAllArtistMusicsAsync(artistId);
    }
}