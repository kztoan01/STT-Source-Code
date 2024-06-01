using Microsoft.AspNetCore.Mvc;
using Sync.DTOs;
using Sync.Services;
using Sync.Services.iml;
using System;
using System.Collections.Generic;

namespace Sync.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaylistController : ControllerBase
    {
        private readonly IPlayListService _playlistService;

        public PlaylistController(IPlayListService playlistService)
        {
            _playlistService = playlistService;
        }

        [HttpPost("AddPlaylist")]
        public ActionResult<PlaylistDTO> AddPlaylist([FromBody] PlaylistDTO playlistDTO)
        {
            var result = _playlistService.AddPlaylist(playlistDTO);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest("Failed to add playlist");
        }

        [HttpPut("UpdatePlaylist")]
        public ActionResult<PlaylistDTO> UpdatePlaylist([FromBody] PlaylistDTO playlistDTO)
        {
            var result = _playlistService.UpdatePlaylist(playlistDTO);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest("Failed to update playlist");
        }

        [HttpDelete("DeletePlaylist/{playlistId}")]
        public IActionResult DeletePlaylist(Guid playlistId)
        {
            try
            {
                _playlistService.DeletePlaylist(playlistId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetPlaylistById/{playlistId}")]
        public ActionResult<PlaylistDTO> GetPlaylistById(Guid playlistId)
        {
            var result = _playlistService.GetPlaylistById(playlistId);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("Playlist not found");
        }

        [HttpGet("GetAllPlaylists")]
        public ActionResult<List<PlaylistDTO>> GetAllPlaylists()
        {
            var result = _playlistService.GetAllPlaylists();
            return Ok(result);
        }

        [HttpGet("GetTotalPlaylists")]
        public ActionResult<int> GetTotalPlaylists()
        {
            var result = _playlistService.GetTotalPlaylists();
            return Ok(result);
        }

        [HttpGet("GetPlaylistsCreatedInLastMonth")]
        public ActionResult<int> GetPlaylistsCreatedInLastMonth()
        {
            var result = _playlistService.GetPlaylistsCreatedInLastMonth();
            return Ok(result);
        }

        [HttpGet("GetMostPopularPlaylist")]
        public ActionResult<PlaylistDTO> GetMostPopularPlaylist()
        {
            var result = _playlistService.GetMostPopularPlaylist();
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("No popular playlist found");
        }

        [HttpGet("GetUserPlaylistsByUserId/{userId}")]
        public ActionResult<List<UserPlaylistDTO>> GetUserPlaylistsByUserId(Guid userId)
        {
            var result = _playlistService.GetUserPlaylistsByUserId(userId);
            return Ok(result);
        }
    }
}
