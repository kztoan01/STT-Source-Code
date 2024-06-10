using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using sync_service.Data;
using sync_service.Interfaces;
using sync_service.Models;

namespace sync_service.Repository
{
    public class MusicRepository : IMusicRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName = "sync-music-storage";

        public MusicRepository(ApplicationDBContext context, IAmazonS3 s3Client)
        {
            _context = context;
            _s3Client = s3Client;
        }
        public async Task<Music> UploadMusicAsync(Music music, IFormFile fileMusic, IFormFile fileImage)
        {
            await UploadedFiles(fileMusic);
            var fileMusicPath = $"{fileMusic.FileName}";

            await UploadedFiles(fileImage);
            var fileImagePath = $"{fileImage.FileName}";


            var musicModel = new Music{
                Id = music.Id,
                musicTitle = music.musicTitle,
                musicUrl = GetPreSignedURL(fileMusicPath),
                musicPicture = GetPreSignedURL(fileImagePath),
                musicPlays = music.musicPlays,
                musicDuration = music.musicDuration,
                releaseDate = music.releaseDate,
                albumId = music.albumId,
                Album = music.Album,
                artistId = music.artistId,
                Artist = music.Artist,
                genreId = music.genreId,
                Genre = music.Genre,
                playlistMusics = music.playlistMusics
            };
            
            await _context.Musics.AddAsync(musicModel);
            await _context.SaveChangesAsync();

            return musicModel;

        }

        public async Task UploadedFiles(IFormFile file)
        {
            var fileTransferUtility = new TransferUtility(_s3Client);
            var filePath = $"{file.FileName}";

            using (var stream = file.OpenReadStream())
            {
                await fileTransferUtility.UploadAsync(stream, _bucketName, filePath);
            }
        }

        public string GetPreSignedURL(string filePath)
        {
            var url = _s3Client.GetPreSignedURL(new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = filePath,
                Expires = DateTime.UtcNow.AddMinutes(30)
            });

            return url;
        }
    }
}