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
        private readonly IMusicService _musicService;

        public MusicController(IMusicService musicService)
        {
            _musicService = musicService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddNewMusic([FromForm] AddMusicDTO music)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var musicModel = music.ToMusicFromCreate();
            var newMusic = await _musicService.UploadMusicAsync(musicModel, music.fileMusic, music.fileImage);
            return Ok(newMusic);
        }
    }
}