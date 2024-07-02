using core.Dtos.Album;
using core.Dtos.Playlist;
using core.Models;
using core.Objects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

    [HttpPost("getUserPlaylist")]
    [Authorize]
    public async Task<IActionResult> GetUserPlaylist(QueryObject queryObject)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userManager.GetUserAsync(User);
        if (user != null)
        {
            var userPlaylist = await _playlistService.GetUserPlaylistsAsync(user.Id, queryObject);

            return Ok(userPlaylist);
        }

        return NotFound("User not found");
    }

    [HttpGet("getPlaylistById/{id:Guid}")]
    public async Task<IActionResult> GetPlaylistById([FromRoute] Guid id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var playlist = await _playlistService.GetPlaylistByIdAsync(id);

        if (playlist == null)
            return NotFound();

        return Ok(playlist);
    }

    [HttpPost("getPlaylistByGenreName")]
    [Authorize]
    public async Task<IActionResult> GetPlaylistsByGenreName(string genreName, QueryObject queryObject)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var playlists = await _playlistService.GetPlaylistsByGenreNameAsync(genreName, queryObject);

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
    [Authorize]
    public async Task<string> AddMusicIntoPlaylist(Guid musicId, Guid playlistId)
    {
        return await _playlistService.AddMusicIntoPlaylistAsync(musicId, playlistId);
    }

    [HttpPost("AddEntireAlbumIntoPlaylist")]
    [Authorize]
    public async Task<string> AddEntireAlbumIntoPlaylist(Guid albumId, Guid playlistId)
    {
        return await _playlistService.AddEntireAlbumIntoPlaylistAsync(albumId, playlistId);
    }

    [HttpGet("GetAlbumByContainArtistByArtistId/{artistId}")]
    [Authorize]
    public async Task<List<AlbumDTO>> GetAlbumByContainArtistByArtistId([FromRoute] Guid artistId)
    {
        return await _playlistService.GetAlbumByContainArtistByArtistId(artistId);
    }

    [HttpDelete("DeleteAMusicInPlaylist")]
    [Authorize]
    public async Task<string> DeleteAMusicInPlaylist(Guid musicId, Guid playlistId)
    {
        return await _playlistService.DeleteMusicInPlaylistAsync(musicId, playlistId);
    }


    [HttpPut("ChangeMusicPositionInPlaylist")]
    [Authorize]
    public async Task<string> ChangeMusicPositionInPlaylist(Guid musicId1, int newPosition, Guid playlistId)
    {
        return await _playlistService.ChangeMusicPositionInPlaylistAsync(musicId1, newPosition, playlistId);
    }

    [HttpDelete("DeletePlaylistById")]
    [Authorize]
    public async Task<Playlist?> DeletePlaylistAsync(Guid id)
    {
        return await _playlistService.DeletePlaylistAsync(id);
    }
}