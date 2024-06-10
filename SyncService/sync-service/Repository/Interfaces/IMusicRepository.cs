using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sync_service.Models;

namespace sync_service.Interfaces
{
    public interface IMusicRepository
    {
        Task<Music> UploadMusicAsync(Music music, IFormFile fileMusic, IFormFile fileImage);
    }
}