using core.Dtos.Music;
using core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using repository.Mappers;
using service.Service.Interfaces;

namespace controller.Controllers;

[Route("music-service/api/[controller]")]
[ApiController]
public class MusicController : ControllerBase
{
    private readonly IArtistService _artistService;

    private readonly IElasticService<ElasticMusicDTO> _elasticService;
    private readonly IMusicService _musicService;
    private readonly UserManager<User> _userManager;

    public MusicController(IMusicService musicService, IElasticService<ElasticMusicDTO> elasticService,
        UserManager<User> userManager, IArtistService artistService)
    {
        _musicService = musicService;
        _userManager = userManager;
        _artistService = artistService;
        _elasticService = elasticService;
    }

    [HttpPost("add")]
    [Authorize]
    public async Task<IActionResult> AddNewMusic([FromForm] AddMusicDTO music)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var musicModel = music.ToMusicFromCreate();
        var elasticModel = musicModel.ToElasticFromMusic();
        await _elasticService.CreateDocumentAsync(elasticModel);
        var artist = await _artistService.GetArtistByUserIdAsync(music.artistId);
        musicModel.artistId = artist.Id;
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


    [HttpPost("getMusicByArtistId/{userId:Guid}")]
    [Authorize]
    public async Task<List<MusicDTO>> GetMusicByArtistId([FromRoute] Guid userId)
    {
        var artist = await _artistService.GetArtistByUserIdAsync(userId);
        var artistId = artist.Id;
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
        var user = await _userManager.GetUserAsync(User);
        if (user != null) return await _musicService.Add1ListenTimeWhenMusicIsListenedAsync(musicId, user.Id);

        return "User not found";
    }

    [HttpGet("GetAllMusicHistory")]
    [Authorize]
    public async Task<List<MusicDTO>> GetAllMusicHistoryByUserIdAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user != null) return await _musicService.GetAllMusicHistoryByUserIdAsync(user.Id);

        return null;
    }

    [HttpDelete("deleteMusic/{musicId:guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteAlbum([FromRoute] Guid musicId)
    {
        // only creator can delete music
        var music = await _musicService.GetMusicByMusicIdAsync(musicId);
        if (music == null) return NotFound("Music not found");
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound("User not found");

        var artist = await _artistService.GetArtistByUserIdAsync(Guid.Parse(user.Id));

        if (artist == null || artist.Id == Guid.Empty || !artist.Id.Equals(music.artistId))
            return Forbid("Only creator can delete this music.");

        var deleteMusic = await _musicService.DeleteMusicByIdAsync(musicId);
        return Ok("Music deleted successfully");
    }
}