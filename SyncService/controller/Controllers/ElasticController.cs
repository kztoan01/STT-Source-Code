using core.Dtos.Music;
using Microsoft.AspNetCore.Mvc;
using service.Service.Interfaces;

namespace controller.Controllers;

[Route("music-service/api/[controller]")]
[ApiController]
public class ElasticController : ControllerBase
{
    private readonly IElasticService<ElasticMusicDTO> _elasticService;

    public ElasticController(IElasticService<ElasticMusicDTO> elasticService)
    {
        _elasticService = elasticService;
    }

    [HttpPost("InsertMusicElastic")]
    public async Task<IActionResult> InsertElastic([FromBody] ElasticMusicDTO music)
    {
        await _elasticService.CreateDocumentAsync(music);
        return Ok(music);
    }

    [HttpGet("SearchMusicElastic/{value}")]
    public async Task<IEnumerable<ElasticMusicDTO>> SearchElastic([FromRoute] string value)
    {
        return await _elasticService.SearchDocument("musicTitle", value);
    }
}