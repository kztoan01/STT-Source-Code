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

        Task<MusicDTO> GetMusicByArtistId(Guid id);
    }
}