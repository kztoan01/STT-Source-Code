using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sync_service.Dtos.Music;
using sync_service.Models;

namespace sync_service.Interfaces
{
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
}