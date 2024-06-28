using core.Dtos.Music;
using core.Models;
using Microsoft.AspNetCore.Http;

namespace repository.Repository.Interfaces;

public interface IMusicRepository
{
    Task<Music> UploadMusicAsync(Music music, IFormFile fileMusic, IFormFile fileImage);
    Task<List<MusicDTO>> GetAllMusicAsync();
    Task<MusicDTO> GetMusicById(Guid id);
    Task<int> ListenTimeOnThisYear(Guid musicId);
    Task<int> ListenTimeOnThisMonth(Guid musicId);
    Task<int> ListenTimeOnThisDay(Guid musicId);
    Task<string> Add1ListenTimeWhenMusicIsListened(Guid musicId);
    Task<MusicDTO> GetMusicByArtistId(Guid id);
}