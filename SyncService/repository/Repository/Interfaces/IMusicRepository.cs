using core.Models;
using System;

namespace repository.Repository.Interfaces
{
    public interface IMusicRepository
    {
        Task<Music> CreateMusicAsync(Music music);
        Task<Music?> GetMusicByIdAsync(Guid id);
        Task<List<Music>> GetAllMusicAsync();
        Task<Music?> UpdateMusicAsync(Guid id, Music music);
        Task<bool> DeleteMusicAsync(Guid id);
        Task<int> GetListenCountAsync(Guid musicId, DateTime startDate, DateTime endDate);
        Task<List<Music>> GetMusicByAlbumIdAsync(Guid albumId);
    }
}
