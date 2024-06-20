using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using core.Dtos.Album;
using core.Dtos.Playlist;
using repository.Mappers;
using core.Models;
using service.Service.Interfaces;
using service.Service.Mappers;

namespace sync_service.Controllers;

[Route("music-service/api/[controller]")]
[ApiController]
public class PlaylistController : ControllerBase
{
    private readonly IPlaylistService _playlistService;
    private readonly UserManager<User> _userManager;

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

    [HttpGet("getPlaylistByGenreName/{genreName}")]
    // [Authorize]
    public async Task<IActionResult> GetPlaylistsByGenreName([FromRoute] string genreName)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var playlists = await _playlistService.GetPlaylistsByGenreNameAsync(genreName);

        if (playlists == null)
            return NotFound();

        return Ok(playlists);
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
    public async Task<string> ChangeMusicPositionInPlaylist(Guid musicId1, int newPosition, Guid playlistId)
    {
        return await _playlistService.ChangeMusicPositionInPlaylist(musicId1, newPosition, playlistId);
    }
}