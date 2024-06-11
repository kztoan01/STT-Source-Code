using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using sync_service.Dtos.Playlist;
using sync_service.Interfaces;
using sync_service.Mappers;
using sync_service.Models;
using sync_service.Service.Interfaces;


namespace sync_service.Controllers
{
    [Route("music-service/api/[controller]")]
    [ApiController]
    public class PlaylistController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IPlaylistService _playlistService;

        public PlaylistController(UserManager<User> userManager, IPlaylistService playlistService)
        {
            _userManager = userManager;
            _playlistService = playlistService;
        }

        [HttpGet("getUserPlaylist")]
        [Authorize]
        public async Task<IActionResult> GetUserPlaylist()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var userPlaylist = await _playlistService.GetUserPlaylistAsync(user.Id);

                return Ok(userPlaylist);
            }
            return NotFound("User not found");
        }

        [HttpGet("getPlaylistById/{id:Guid}")]
        [Authorize]
        public async Task<IActionResult> GetPlaylistById([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var playlist = await _playlistService.GetPlaylistByIdAsync(id);

            if (playlist == null)
                return NotFound();

            return Ok(playlist);
        }

        [HttpPost("createPlaylist")]
        [Authorize]
        public async Task<IActionResult> CreatePlaylist([FromBody] CreatePlaylistDTO playlist)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var playlistModel = playlist.ToPlaylistFromCreate(user.Id);
                await _playlistService.CreatePlaylistAsync(playlistModel);
                return CreatedAtAction(nameof(GetPlaylistById), new { id = playlistModel.Id }, playlistModel);
            }
            return NotFound("User not found");
        }
    }


}