using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using sync_service.Dtos.Playlist;
using sync_service.Interfaces;
using sync_service.Mappers;
using sync_service.Models;


namespace sync_service.Controllers
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

        [HttpGet("getUserPlaylist")]
        public async Task<IActionResult> GetUserPlaylist([FromRoute] int userId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userPlaylist = await _playlistRepository.GetUserPlaylistAsync(userId);

            return Ok(userPlaylist);
        }

        [HttpGet("getPlaylistById/{id:int}")]
        public async Task<IActionResult> GetPlaylistById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var playlist = await _playlistRepository.GetPlaylistByIdAsync(id);

            if(playlist == null)
                return NotFound();
            
            return Ok(playlist);
        }

        [HttpPost("createPlaylist")]
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