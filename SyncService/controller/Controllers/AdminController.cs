using core.Objects;
using Microsoft.AspNetCore.Mvc;
using service.Service.Interfaces;
using core.Dtos.Album;

namespace controller.Controllers
{
    [Route("music-service/api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("getallartist")]
        public async Task<IActionResult> GetAllArtist([FromQuery] QueryArtist queryObject)
        {
            return Ok(await _adminService.getArtist(queryObject));
        }

        [HttpGet("getuser")]
        public async Task<IActionResult> GetUser([FromQuery] QueryUser queryUser)
        {
            return Ok(await _adminService.getUser(queryUser));
        }

        [HttpGet("getalbum")]
        public async Task<IActionResult> GetAlbum([FromQuery] QueryAlbum queryAlbum)
        {
            var albums = await _adminService.GetAlbum(queryAlbum);
            return Ok(albums);
        }
        [HttpDelete("deleteartist/{id}")]
        public async Task<IActionResult> DeleteArtist(Guid id)
        {
            var result = await _adminService.DeleteArtist(id);
            if (result)
            {
                return Ok(new { message = "Artist deleted successfully." });
            }
            return NotFound(new { message = "Artist not found." });
        }
        
        [HttpDelete("deleteplaylist/{id}")]
        public async Task<IActionResult> DeletePlaylist(Guid id)
        {
            var result = await _adminService.DeletePlaylist(id);
            if (result)
            {
                return Ok(new { message = "Playlist deleted successfully." });
            }
            return NotFound(new { message = "Playlist not found." });
        }
        [HttpDelete("deletemusic/{id}")]
        public async Task<IActionResult> DeleteMusic(Guid id)
        {
            var result = await _adminService.DeleteMusic(id);
            if (result)
            {
                return Ok(new { message = "Music deleted successfully." });
            }
            return NotFound(new { message = "Music not found." });
        }
    }
}