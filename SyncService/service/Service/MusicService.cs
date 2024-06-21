using core.Dtos.Music;
using core.Models;
using Microsoft.AspNetCore.Http;
using repository.Repository.Interfaces;
using service.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Amazon.S3;
using Amazon.S3.Transfer;
using System.IO;
using System.Linq;
using Amazon.S3.Model;
using core.Dtos.Album;

namespace service.Service
{
    public class MusicService : IMusicService
    {
        private readonly IMusicRepository _musicRepository;
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName = "sync-music-storage";

        public MusicService(IMusicRepository musicRepository, IAmazonS3 s3Client)
        {
            _musicRepository = musicRepository;
            _s3Client = s3Client;
        }

        public async Task<MusicDTO> UploadMusicAsync(Music music, IFormFile fileMusic, IFormFile fileImage)
        {
            var imageUrl = await UploadFileAsync(fileImage, music.Id, "image");
            var musicUrl = await UploadFileAsync(fileMusic, music.Id, "music");

            music.musicPicture = imageUrl;
            music.musicUrl = musicUrl;

            var createdMusic = await _musicRepository.CreateMusicAsync(music);

            return ConvertToDto(createdMusic);
        }

        public async Task<List<MusicDTO>> GetAllMusicAsync()
        {
            var musics = await _musicRepository.GetAllMusicAsync();
            return musics.Select(ConvertToDto).ToList();
        }

        public async Task<MusicDTO?> GetMusicByIdAsync(Guid id)
        {
            var music = await _musicRepository.GetMusicByIdAsync(id);
            return music != null ? ConvertToDto(music) : null;
        }

        public async Task<MusicDTO?> GetMusicByArtistIdAsync(Guid artistId)
        {
            var musics = await _musicRepository.GetAllMusicAsync();
            var music = musics.FirstOrDefault(m => m.artistId == artistId);
            return music != null ? ConvertToDto(music) : null;
        }

        public async Task<string> Add1ListenTimeWhenMusicIsListenedAsync(Guid musicId)
        {
            var music = await _musicRepository.GetMusicByIdAsync(musicId);
            if (music == null)
                return "Music not found.";

            music.musicPlays++;

            var today = DateTime.UtcNow.Date; 

            var musicListen = music.MusicListens?.Find(ml => ml.ListenDate.Date == today);

            if (musicListen == null)
            {
                var newMusicListen = new MusicListen()
                {
                    ListenDate = today,
                    ListenCount = 1,
                    MusicId = musicId
                };
                if (music.MusicListens == null)
                {
                    music.MusicListens = new List<MusicListen>();
                }
                music.MusicListens.Add(newMusicListen);
            }
            else
            {
                musicListen.ListenCount++;
            }

            await _musicRepository.UpdateMusicAsync(musicId, music);

            return "Listen time added.";
        }


        public async Task<int> ListenTimeOnThisDayAsync(Guid musicId)
        {
            return await GetListenCountAsync(musicId, DateTime.UtcNow.Date, DateTime.UtcNow.Date);
        }

        public async Task<int> ListenTimeOnThisMonthAsync(Guid musicId)
        {
            var firstDayOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            return await GetListenCountAsync(musicId, firstDayOfMonth, DateTime.UtcNow);
        }

        public async Task<int> ListenTimeOnThisYearAsync(Guid musicId)
        {
            var firstDayOfYear = new DateTime(DateTime.UtcNow.Year, 1, 1);
            return await GetListenCountAsync(musicId, firstDayOfYear, DateTime.UtcNow);
        }

        private async Task<int> GetListenCountAsync(Guid musicId, DateTime startDate, DateTime endDate)
        {
            var listenCount = await _musicRepository.GetListenCountAsync(musicId, startDate, endDate);
            return listenCount;
        }

        private async Task<string> UploadFileAsync(IFormFile file, Guid musicId, string fileType)
        {
            var fileTransferUtility = new TransferUtility(_s3Client);
            var fileExtension = Path.GetExtension(file.FileName);
            var filePath = $"{fileType}/{Guid.NewGuid()}{fileExtension}";

            using (var stream = file.OpenReadStream())
            {
                await fileTransferUtility.UploadAsync(stream, _bucketName, filePath);
            }

            var url = _s3Client.GetPreSignedURL(new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = filePath,
                Expires = DateTime.UtcNow.AddMinutes(30)
            });

            return url;
        }


        private MusicDTO ConvertToDto(Music music)
        {
            return new MusicDTO
            {
                Id = music.Id,
                genreName = music.Genre.genreName,
                musicDuration = music.musicDuration,
                musicPicture = music.musicPicture,
                musicPlays = music.musicPlays,
                musicTitle = music.musicTitle,
                musicUrl = music.musicUrl,
                releaseDate = music.releaseDate,
                AlbumDTO = new AlbumDTO
                {
                    Id = music.Album.Id,
                    albumTitle = music.Album.albumTitle
                },
                artistName = music.Artist.User.userFullName
            };
        }
    }
}
