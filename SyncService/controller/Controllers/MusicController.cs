using Microsoft.AspNetCore.Mvc;
using core.Dtos.Music;
using repository.Mappers;
using service.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace controller.Controllers;

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
    [Authorize]
    public async Task<IActionResult> AddNewMusic([FromForm] AddMusicDTO music)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var musicModel = music.ToMusicFromCreate();
        var newMusic = await _musicService.UploadMusicAsync(musicModel, music.fileMusic, music.fileImage);
        return Ok(newMusic);
    }

    [HttpGet("all")]
    [Authorize]
    public async Task<IActionResult> GetAllMusic()
    {
        var allMusic = await _musicService.GetAllMusicAsync();
        return Ok(allMusic);
    }


    [HttpGet("getMusicById/{musicId:Guid}")]
    [Authorize]
    public async Task<IActionResult> GetMusicById([FromRoute] Guid musicId)
    {
        var music = await _musicService.GetMusicByIdAsync(musicId);
        return Ok(music);
    }


    [HttpPost("getMusicByArtistId/{artistId:Guid}")]
    [Authorize]
    public async Task<MusicDTO> GetMusicByArtistId([FromRoute] Guid artistId)
    {
        return await _musicService.GetMusicByArtistIdAsync(artistId);
    }


    [HttpGet("ListenTimeOnThisYear/{musicId:Guid}")]
    [Authorize]
    public async Task<int> ListenTimeOnThisYear([FromRoute] Guid musicId)
    {
        return await _musicService.ListenTimeOnThisYearAsync(musicId);
    }

    [HttpGet("ListenTimeOnThisMonth/{musicId:guid}")]
    [Authorize]
    public async Task<int> ListenTimeOnThisMonth([FromRoute] Guid musicId)
    {
        return await _musicService.ListenTimeOnThisMonthAsync(musicId);
    }

    [HttpGet("ListenTimeOnThisDay/{musicId:guid}")]
    [Authorize]
    public async Task<int> ListenTimeOnThisDay([FromRoute] Guid musicId)
    {
        return await _musicService.ListenTimeOnThisDayAsync(musicId);
    }

    [HttpPut("Add1ListenTimeWhenMusicIsListened")]
    [Authorize]
    public async Task<string> Add1ListenTimeWhenMusicIsListened(Guid musicId)
    {
        return await _musicService.Add1ListenTimeWhenMusicIsListenedAsync(musicId);
    }
}