using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sync_service.Dtos.Music;
using sync_service.Interfaces;
using sync_service.Models;
using sync_service.Service.Interfaces;

namespace sync_service.Service
{
    public class MusicService : IMusicService
    {
        private readonly IMusicRepository _musicRepository;

        public MusicService(IMusicRepository musicRepository)
        {
            _musicRepository = musicRepository;
        }

        public async Task<Music> UploadMusicAsync(Music music, IFormFile fileMusic, IFormFile fileImage)
        {
            return await _musicRepository.UploadMusicAsync(music, fileMusic, fileImage);
        }

        public async Task<List<MusicDTO>> GetAllMusicAsync()
        {
            return await _musicRepository.GetAllMusicAsync();
        }

        public async Task<MusicDTO> GetMusicById(Guid id)
        {
            return await _musicRepository.GetMusicById(id);
        }

        public async Task<MusicDTO> GetMusicByArtistId(Guid id)
        {
            return await _musicRepository.GetMusicByArtistId(id);
        }

        public async Task<int> ListenTimeOnThisYear(Guid musicId)

        {
           return await _musicRepository.ListenTimeOnThisYear(musicId);
        }

        public async Task<int> ListenTimeOnThisMonth(Guid musicId)
        {
            return await _musicRepository.ListenTimeOnThisMonth(musicId);
        }

        public async Task<int> ListenTimeOnThisDay(Guid musicId)
        {
            return await _musicRepository.ListenTimeOnThisDay(musicId);
        }

        public async Task<string> Add1ListenTimeWhenMusicIsListened(Guid musicId)
        {
            return await _musicRepository.Add1ListenTimeWhenMusicIsListened(musicId);
        }

    }
}