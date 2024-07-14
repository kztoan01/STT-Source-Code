using core.Objects;
using Microsoft.AspNetCore.Mvc;
using service.Service.Interfaces;
using core.Dtos.Album;

namespace controller.Controllers
{
    [Route("music-service/api/[controller]")]
    [ApiController]
    public class AdminController(IAdminService adminService) : ControllerBase
    {
        [HttpGet("getallartist")]
        public async Task<IActionResult> GetAllArtist([FromQuery] QueryArtist queryObject)
        {
            return Ok(await adminService.getArtist(queryObject));
        }

        [HttpGet("getuser")]
        public async Task<IActionResult> GetUser([FromQuery] QueryUser queryUser)
        {
            return Ok(await adminService.getUser(queryUser));
        }

        [HttpGet("getalbum")]
        public async Task<IActionResult> GetAlbum([FromQuery] QueryAlbum queryAlbum)
        {
            var albums = await adminService.GetAlbum(queryAlbum);
            return Ok(albums);
        }
        [HttpDelete("deleteartist/{id}")]
        public async Task<IActionResult> DeleteArtist(Guid id)
        {
            var result = await adminService.DeleteArtist(id);
            if (result)
            {
                return Ok(new { message = "Artist deleted successfully." });
            }
            return NotFound(new { message = "Artist not found." });
        }
        
        [HttpDelete("deleteplaylist/{id}")]
        public async Task<IActionResult> DeletePlaylist(Guid id)
        {
            var result = await adminService.DeletePlaylist(id);
            if (result)
            {
                return Ok(new { message = "Playlist deleted successfully." });
            }
            return NotFound(new { message = "Playlist not found." });
        }
        [HttpDelete("deletemusic/{id}")]
        public async Task<IActionResult> DeleteMusic(Guid id)
        {
            var result = await adminService.DeleteMusic(id);
            if (result)
            {
                return Ok(new { message = "Music deleted successfully." });
            }
            return NotFound(new { message = "Music not found." });
        }
    }
}