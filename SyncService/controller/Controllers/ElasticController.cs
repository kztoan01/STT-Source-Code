using core.Dtos.Music;
using Microsoft.AspNetCore.Mvc;
using service.Service.Interfaces;

namespace controller.Controllers;

[Route("music-service/api/[controller]")]
[ApiController]
public class ElasticController(IElasticService<ElasticMusicDTO> elasticService) : ControllerBase
{
    [HttpPost("InsertMusicElastic")]
    public async Task<IActionResult> InsertElastic([FromBody] ElasticMusicDTO music)
    {
        await elasticService.CreateDocumentAsync(music);
        return Ok(music);
    }

    [HttpGet("SearchMusicElastic/{value}")]
    public async Task<IEnumerable<ElasticMusicDTO>> SearchElastic([FromRoute] string value)
    {
        return await elasticService.SearchDocument("musicTitle", value);
    }
}