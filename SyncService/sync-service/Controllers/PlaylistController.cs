using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using sync_service.Dtos.Album;
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

        [HttpPost("addMusicIntoPlaylist")]
        public async Task<string> AddMusicIntoPlaylist(Guid musicId, Guid playlistId)
        {
            return await _playlistService.AddMusicIntoPlaylist(musicId, playlistId);
        }

        [HttpPost("AddEntireAlbumIntoPlaylist")]
        public async Task<string> AddEntireAlbumIntoPlaylist(Guid albumId, Guid playlistId)
        {
            return await _playlistService.AddEntireAlbumIntoPlaylist(albumId, playlistId);
        }

        [HttpPost("GetAlbumByContainArtistByArtistId")]
        public async Task<List<AlbumDTO>> GetAlbumByContainArtistByArtistId(Guid artistId)
        {
            return await _playlistService.GetAlbumByContainArtistByArtistId(artistId);
        }

        [HttpPost("DeleteAMusicInPlaylist")]
        public async Task<string> DeleteAMusicInPlaylist(Guid musicId, Guid playlistId)
        {
            return await _playlistService.DeleteAMusicInPlaylist(musicId, playlistId);
        }


        [HttpPost("ChangeMusicPositionInPlaylist")]
        public async Task<string> ChangeMusicPositionInPlaylist(Guid musicId1, Guid musicId2, Guid playlistId)
        {
            return await _playlistService.ChangeMusicPositionInPlaylist(musicId1, musicId2, playlistId);
        }
    }


}