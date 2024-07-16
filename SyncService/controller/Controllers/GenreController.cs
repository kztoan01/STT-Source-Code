using Microsoft.AspNetCore.Mvc;
using service.Service.Interfaces;

namespace controller.Controllers;

[Route("music-service/api/[controller]")]
[ApiController]
public class GenreController : ControllerBase
{
    private readonly IGenreService _genreService;

    public GenreController(IGenreService genreService)
    {
        _genreService = genreService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllGenre()
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var listGenre = await _genreService.GetAllGenre();

        return Ok(listGenre);
    }
}