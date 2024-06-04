using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using sync_music_service.Dtos.Playlist;
using sync_music_service.Interfaces;
using sync_music_service.Mappers;
using sync_music_service.Models;


namespace sync_music_service.Controllers
{
    [Route("music-service/api/[controller]")]
    [ApiController]
    public class PlaylistController : ControllerBase
    {
        private readonly IPlaylistRepository _playlistRepository;

        public PlaylistController(IPlaylistRepository playlistRepository)
        {
            _playlistRepository = playlistRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserPlaylist([FromRoute] int userId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userPlaylist = await _playlistRepository.GetUserPlaylistAsync(userId);

            return Ok(userPlaylist);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPlaylistById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var playlist = await _playlistRepository.GetPlaylistByIdAsync(id);

            if(playlist == null)
                return NotFound();
            
            return Ok(playlist);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlaylist([FromBody] CreatePlaylistDTO playlist)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var playlistModel = playlist.ToPlaylistFromCreate();
            await _playlistRepository.CreatePlaylistAsync(playlistModel);

            return CreatedAtAction(nameof(GetPlaylistById), new { id = playlistModel.Id }, playlistModel);
        }
    }


}