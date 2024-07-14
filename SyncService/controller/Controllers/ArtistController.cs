using core.Dtos.Artist;
using core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using service.Service.Interfaces;

namespace controller.Controllers;

[Route("music-service/api/[controller]")]
[ApiController]
public class ArtistController : ControllerBase
{
    private readonly IArtistService _artistService;
    private readonly UserManager<User> _userManager;

    public ArtistController(UserManager<User> userManager, IArtistService artistService)
    {
        _userManager = userManager;
        _artistService = artistService;
    }


    [HttpPost("GetArtistDTOById")]
    public async Task<ArtistDTO> GetArtistDTOById(Guid id)
    {
        return await _artistService.GetArtistDTOById(id);
    }

    [HttpPut("updateArtistInfor/{userId}")]
    [Authorize]
    public async Task<IActionResult> UpdateArtistImage([FromRoute] Guid userId, [FromForm] ArtistImageDTO artistImage)
    {
        if (artistImage.image == null || artistImage.image.Length == 0)
        {
            return BadRequest("Image is not valid.");
        }

        var result = await _artistService.UpdateArtistInforAsync(userId,artistImage);

        if (result)
        {
            return Ok("Artist's Information updated successfully.");
        }

        return StatusCode(404, "Id not found");
    }


    [HttpGet("getAllArtistMusics/{artistId}")]
    //[Authorize]
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