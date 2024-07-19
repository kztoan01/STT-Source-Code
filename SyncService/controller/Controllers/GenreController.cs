using core.Dtos.Genre;
using core.Models;
using core.Objects;
using Elastic.Clients.Elasticsearch.Sql;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using service.Service;
using service.Service.Interfaces;

namespace controller.Controllers;

[Route("music-service/api/[controller]")]
[ApiController]
public class GenreController : ControllerBase
{
    private readonly IGenreService _genreService;
    private readonly UserManager<User> _userManager;

    public GenreController(IGenreService genreService, UserManager<User> userManager)
    {
        _genreService = genreService;
        _userManager = userManager;
    }

    [HttpGet("GetAllGenres")]

    public async Task<IActionResult> GetAllGenres([FromQuery] QueryObject queryObject)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var listGenre = await _genreService.GetAllGenre(queryObject);

        return Ok(listGenre);
    }

    [HttpPost("createGenre")]
    // [Authorize]
    public async Task<IActionResult> CreateGenre([FromForm] GenreDTO genre)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        //var user = await _userManager.GetUserAsync(User);
        //if (user == null) return NotFound("User not found");

        await _genreService.CreateGenreAsync(genre);
        return Ok("Genre created successfully");
    }


    [HttpPut("updateGenre/{genreId:Guid}")]

    public async Task<IActionResult> UpdateGenreById([FromForm] GenreDTO genre, [FromRoute] Guid genreId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var checkGenreId = await _genreService.GetGenreByIdAsync(genreId);
        if (checkGenreId == null)
        {
            return NotFound("Invalid genreId or genreId not found!");
        }
        var rsGenre = await _genreService.UpdateGenreByIdAsync(genre, genreId);
        return Ok("Genre updated successfully");
    }

    [HttpDelete("deleteGenre/{genreId:Guid}")]
   //[Authorize]
    public async Task<IActionResult> DeleteGenre([FromRoute] Guid genreId)
    {
        var genre = await _genreService.GetGenreByIdAsync(genreId);
        if (genre == null) return NotFound("Album not found");

        var deletedAlbum = await _genreService.DeleteGenreByIdAsync(genreId);
        if (deletedAlbum) return Ok("Genre deleted successfully");
        return StatusCode(500, "An error occurred while deleting the Genre");
    }

}