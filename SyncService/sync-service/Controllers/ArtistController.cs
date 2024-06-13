using Microsoft.AspNetCore.Mvc;
using sync_service.Dtos.Artist;
using sync_service.Service.Interfaces;

namespace sync_service.Controllers
{
    public class ArtistController : Controller
    {

        private readonly IArtistService _artistService;

        public ArtistController(IArtistService artistService)
        {
            _artistService = artistService;
        }

        [HttpPost("GetArtistDTOById")]
        public async Task<ArtistDTO> GetArtistDTOById(Guid id)
        {
            return await _artistService.GetArtistDTOById(id);
        }
    }
}
