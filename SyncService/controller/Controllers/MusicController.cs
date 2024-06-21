using Microsoft.AspNetCore.Mvc;
using core.Dtos.Music;
using repository.Mappers;
using service.Service.Interfaces;

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
    public async Task<IActionResult> AddNewMusic([FromForm] AddMusicDTO music)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var musicModel = music.ToMusicFromCreate();
        var newMusic = await _musicService.UploadMusicAsync(musicModel, music.fileMusic, music.fileImage);
        return Ok(newMusic);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllMusic()
    {
        var allMusic = await _musicService.GetAllMusicAsync();
        return Ok(allMusic);
    }


    [HttpPost("getMusicById")]
    public async Task<IActionResult> GetMusicById(Guid id)
    {
        var music = await _musicService.GetMusicById(id);
        return Ok(music);
    }

    [HttpPost("getMusicByArtistId")]
    public async Task<MusicDTO> GetMusicByArtistId(Guid id)
    {
        return await _musicService.GetMusicByArtistId(id);
    }


    [HttpPost("ListenTimeOnThisYear")]
    public async Task<int> ListenTimeOnThisYear(Guid musicId)
    {
        return await _musicService.ListenTimeOnThisYear(musicId);
    }

    [HttpPost("ListenTimeOnThisMonth")]
    public async Task<int> ListenTimeOnThisMonth(Guid musicId)
    {
        return await _musicService.ListenTimeOnThisMonth(musicId);
    }

    [HttpPost("ListenTimeOnThisDay")]
    public async Task<int> ListenTimeOnThisDay(Guid musicId)
    {
        return await _musicService.ListenTimeOnThisDay(musicId);
    }

    [HttpPost("Add1ListenTimeWhenMusicIsListened")]
    public async Task<string> Add1ListenTimeWhenMusicIsListened(Guid musicId)
    {
        return await _musicService.Add1ListenTimeWhenMusicIsListened(musicId);
    }
}