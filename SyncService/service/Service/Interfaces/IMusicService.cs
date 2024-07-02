using core.Dtos.Music;
using core.Models;
using Microsoft.AspNetCore.Http;

namespace service.Service.Interfaces
{
    public interface IMusicService
    {
        Task<MusicDTO> UploadMusicAsync(Music music, IFormFile fileMusic, IFormFile fileImage);
        Task<List<MusicDTO>> GetAllMusicAsync();

        Task<List<MusicDTO>> GetAllMusicHistoryByUserIdAsync(string userId);

        Task<MusicDTO?> GetMusicByIdAsync(Guid id);
        Task<string> Add1ListenTimeWhenMusicIsListenedAsync(Guid musicId,string userId);
        Task<int> ListenTimeOnThisYearAsync(Guid musicId);
        Task<int> ListenTimeOnThisMonthAsync(Guid musicId);
        Task<int> ListenTimeOnThisDayAsync(Guid musicId);
        Task<MusicDTO?> GetMusicByArtistIdAsync(Guid artistId);
        Task<bool> DeleteMusicByIdAsync(Guid musicId);

        Task<Music> GetMusicByMusicIdAsync(Guid musicId);
    }
}
