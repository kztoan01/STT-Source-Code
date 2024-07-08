﻿using core.Dtos.Album;
using core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using service.Service.Interfaces;

namespace sync_service.Controllers;

[Route("music-service/api/[controller]")]
[ApiController]
public class AlbumController : ControllerBase
{
    private readonly IAlbumService _albumService;
    private readonly IArtistService _artistService;
    private readonly UserManager<User> _userManager;
    private readonly IUserService _userService;

    public AlbumController(UserManager<User> userManager, IAlbumService albumService, IUserService userService,
        IArtistService artistService)
    {
        _userManager = userManager;
        _albumService = albumService;
        _userService = userService;
        _artistService = artistService;
    }

    [HttpGet("getAlbumsByGenreName/{genreName}")]
    //[Authorize]
    public async Task<IActionResult> GetAlbumByGenreName([FromRoute] string genreName)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var albums = await _albumService.getAlbumByGenreNameAsync(genreName);

        if (albums == null)
            return NotFound();

        return Ok(albums);
    }

    [HttpGet("getAllArtistAlbums/{artistId}")]
    //[Authorized]
    public async Task<IActionResult> GetAllArtistAlbumsAsync([FromRoute] Guid artistId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var albums = await _albumService.GetAllArtistAlbumsAsync(artistId);


        if (albums == null)
            return NotFound();

        return Ok(albums);
    }

    [HttpPost("createAlbum")]
    [Authorize]
    public async Task<IActionResult> CreateAlbum([FromBody] CreateAlbumDTO album)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound("User not found");

        var artist = await _artistService.GetArtistByUserIdAsync(Guid.Parse(user.Id));

        if (artist == null || artist == null || artist.Id == Guid.Empty)
            return Forbid("Only artists can create albums.");

        await _albumService.CreateAlbumAsync(album, artist.Id);
        return Ok("Album created successfully");
    }


    [HttpPut("updateAlbum/{albumId:guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteAlbum([FromBody] CreateAlbumDTO albumDTO, [FromRoute] Guid albumId)
    {
        var album = await _albumService.GetAlbumByIdAsync(albumId);
        if (album == null) return NotFound("Album not found");
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound("User not found");

        var artist = await _artistService.GetArtistByUserIdAsync(Guid.Parse(user.Id));

        if (artist == null || artist.Id == Guid.Empty) return Forbid("Only artists can update albums.");
        var updateAlbum = await _albumService.EditAlbumAsync(albumDTO, artist.Id, albumId);
        return Ok("Album deleted successfully");
    }


    [HttpDelete("deleteAlbum/{albumId:guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteAlbum([FromRoute] Guid albumId)
    {
        var album = await _albumService.GetAlbumByIdAsync(albumId);
        if (album == null) return NotFound("Album not found");

        var deletedAlbum = await _albumService.DeleteAlbumAsync(albumId);
        return Ok("Album deleted successfully");
    }


    [HttpGet("getAllAlbums")]
    //[Authorize]
    public async Task<IActionResult> GetAllAlbums()
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var albums = await _albumService.getAllAlbumsAsync(null);

        if (albums == null)
            return NotFound();

        return Ok(albums);
    }

    [HttpGet("getMostViewAlbums")]
    //[Authorize]
    public async Task<IActionResult> GetMostViewAlbum()
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var albums = await _albumService.GetMostListenAlbum();

        if (albums == null)
            return NotFound();

        return Ok(albums);
    }

    [HttpGet("getAlbumDetail/{albumId:Guid}")]
    //[Authorize]
    public async Task<IActionResult> GetAlbumDetail([FromRoute] Guid albumId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var albums = await _albumService.GetAlbumDetail(albumId);

        if (albums == null)
            return NotFound();

        return Ok(albums);
    }
}