using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.EntityFrameworkCore;
using sync_service.Data;
using sync_service.Dtos.Album;
using sync_service.Dtos.Music;
using sync_service.Interfaces;
using sync_service.Mappers;
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
            var imageUrl = await UploadedFiles(fileImage, music.Id, "image");
            var musicUrl = await UploadedFiles(fileMusic, music.Id, "music");

            var musicModel = new Music
            {
                Id = music.Id,
                musicTitle = music.musicTitle,
                musicUrl = musicUrl,
                musicPicture = imageUrl,
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

        public async Task<string> UploadedFiles(IFormFile file, Guid musicId, string fileType)
        {
            var fileTransferUtility = new TransferUtility(_s3Client);
            var fileExtension = Path.GetExtension(file.FileName);
            var filePath = $"{fileType}{Guid.NewGuid()}{fileExtension}";

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
        public async Task<List<MusicDTO>> GetAllMusicAsync()
        {
            return await _context.Musics
                .Include(m => m.Album)
                .Include(m => m.Artist)
                .Include(m => m.Genre).Select(m => new MusicDTO
                {
                    Id = m.Id,
                    genreName = m.Genre.genreName,
                    musicDuration = m.musicDuration,
                    musicPicture = m.musicPicture,
                    musicPlays = m.musicPlays,
                    musicTitle = m.musicTitle,
                    musicUrl = m.musicUrl,
                    releaseDate = m.releaseDate,
                    AlbumDTO = new AlbumDTO
                    {
                        Id = m.Album.Id,
                        albumTitle = m.Album.albumTitle
                    },
                    artistName = m.Artist.User.userFullName,

                })
                .ToListAsync();
        }

        public async Task<MusicDTO> GetMusicById(Guid id)
        {
            Music music = await _context.Musics
                .Include(m => m.Album)
                .Include(m => m.Artist)
                .Include(m => m.Genre)
                .Include(m => m.Artist.User)
                .FirstOrDefaultAsync(m => m.Id.Equals(id));

            if (music == null)
            {
                return null; 
            }

            var musicDTO = MusicMapper.Convert(music);

            return musicDTO;
        }

        public async Task<MusicDTO> GetMusicByArtistId(Guid id)
        {
            Music music = await _context.Musics
                .Include(m => m.Album)
                .Include(m => m.Artist)
                .Include(m => m.Genre)
                .Include(m => m.Artist.User)
                .FirstOrDefaultAsync(m => m.artistId.Equals(id));

            if (music == null)
            {
                return null;
            }

            var musicDTO = MusicMapper.Convert(music);

            return musicDTO;
        }






    }
}