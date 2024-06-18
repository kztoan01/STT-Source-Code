using Microsoft.AspNetCore.Authorization;
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
    public class AlbumController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IAlbumService _albumService;
        public AlbumController(UserManager<User> userManager, IAlbumService albumService)
        {
            _userManager = userManager;
            _albumService = albumService;
        }

        [HttpGet("getAlbumByGenreName/{genreName}")]
        //[Authorize]
        public async Task<IActionResult> GetAlbumByGenreName([FromRoute] string genreName)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var albums = await _albumService.getAlbumByGenreNameAsync(genreName);

            if (albums == null)
                return NotFound();

            return Ok(albums);
        }


        [HttpGet("getAllAlbums")]
        //[Authorize]
        public async Task<IActionResult> GetAllAlbums()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var albums = await _albumService.getAllAlbumsAsync();

            if (albums == null)
                return NotFound();

            return Ok(albums);
        }
        [HttpGet("getMostViewAlbums")]
        //[Authorize]
        public async Task<IActionResult> GetMostViewAlbum()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var albums = await _albumService.GetMostListenAlbum();

            if (albums == null)
                return NotFound();

            return Ok(albums);
        }

        [HttpGet("getAlbumDetail/{albumId:Guid}")]
        //[Authorize]
        public async Task<IActionResult> GetAlbumDetail([FromRoute]Guid albumId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var albums = await _albumService.GetAlbumDetail(albumId);

            if (albums == null)
                return NotFound();

            return Ok(albums);
        }
    }
}
