using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using sync_service.Models;
using sync_service.Service;
using sync_service.Service.Interfaces;

namespace sync_service.Controllers
{
    [Route("music-service/api/[controller]")]
    [ApiController]
    public class ArtistController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IArtistService _artistService;
        public ArtistController(UserManager<User> userManager, IArtistService artistService)
        {
            _userManager = userManager;
            _artistService = artistService;
        }

        [HttpGet("getAllArtistAlbums/{artistId}")]
        //[Authorized]
        public async Task<IActionResult> GetAllArtistAlbumsAsync([FromRoute] Guid artistId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var albums = await _artistService.GetAllArtistAlbumsAsync(artistId);


            if (albums == null)
                return NotFound();

            return Ok(albums);
        }

        [HttpPost("GetArtistDTOById")]
        public async Task<ArtistDTO> GetArtistDTOById(Guid id)
        {
            return await _artistService.GetArtistDTOById(id);
        }

        [HttpGet("getAllArtistMusics/{artistId}")]
        //[Authorized]
        public async Task<IActionResult> GetAllArtistMusicsAsync([FromRoute] Guid artistId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var musics = await _artistService.GetAllArtistMusicsAsync(artistId);


            if (musics == null)
                return NotFound();

            return Ok(musics);
        }
    }
}
