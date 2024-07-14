using core.Dtos.Artist;
using core.Models;
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

    [HttpPut("uploadArtistImage")]
    public async Task<IActionResult> UpdateArtistImage([FromForm] ArtistImageDTO artistImage)
    {
        if (artistImage.image == null || artistImage.image.Length == 0)
        {
            return BadRequest("Image is not valid.");
        }

        var result = await _artistService.UpdateArtistImageAsync(artistImage);

        if (result)
        {
            return Ok("Image updated successfully.");
        }

        return StatusCode(404, "Id not found");
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