using sync_service.Dtos.Artist;
using sync_service.Interfaces;

namespace sync_service.Service
{
    public class ArtistService
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
    }
}
