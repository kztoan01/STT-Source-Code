using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using sync_service.Dtos.Music;
using sync_service.Mappers;
using sync_service.Service.Interfaces;

namespace sync_service.Controllers
{
    [Route("music-service/api/[controller]")]
    [ApiController]
    public class MusicController : ControllerBase
    {
        private readonly ILogger<MusicController> _logger;
        private readonly IMusicService _musicService;

        public MusicController(ILogger<MusicController> logger, IMusicService musicService)
        {
            _logger = logger;
            _musicService = musicService;
        }

        [HttpPost("add-new-music")]
        public async Task<IActionResult> AddNewMusic([FromForm] AddMusicDTO music, [FromForm] IFormFile fileMusic, [FromForm] IFormFile fileImage)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var musicModel = music.ToMusicFromCreate();
            var newMusic = await _musicService.UploadMusicAsync(musicModel, fileMusic, fileImage);
            return Ok(newMusic);
        }
    }
}