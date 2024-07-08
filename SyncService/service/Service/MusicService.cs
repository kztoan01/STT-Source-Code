using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using core.Dtos.Album;
using core.Dtos.Music;
using core.Models;
using Microsoft.AspNetCore.Http;
using repository.Repository.Interfaces;
using service.Service.Interfaces;

namespace service.Service;

public class MusicService : IMusicService
{
    private readonly string _bucketName = "sync-music-storage";
    private readonly IMusicRepository _musicRepository;
    private readonly IAmazonS3 _s3Client;

    public MusicService(IMusicRepository musicRepository, IAmazonS3 s3Client)
    {
        _musicRepository = musicRepository;
        _s3Client = s3Client;
    }

    public async Task<Music> UploadMusicAsync(Music music, IFormFile fileMusic, IFormFile fileImage)
    {
        var imageUrl = await UploadFileAsync(fileImage, music.Id, "image");
        var musicUrl = await UploadFileAsync(fileMusic, music.Id, "music");

        music.musicPicture = imageUrl;
        music.musicUrl = musicUrl;

        var createdMusic = await _musicRepository.CreateMusicAsync(music);

        return createdMusic;
    }

    public async Task<List<MusicDTO>> GetAllMusicAsync()
    {
        var musics = await _musicRepository.GetAllMusicAsync();
        return musics.Select(x => ConvertToDto(x)).ToList();
    }

    public async Task<MusicDTO?> GetMusicByIdAsync(Guid id)
    {
        var music = await _musicRepository.GetMusicByIdAsync(id);
        return music != null ? ConvertToDto(music) : null;
    }

    public async Task<List<MusicDTO>> GetMusicByArtistIdAsync(Guid artistId)
    {
        var musics = await _musicRepository.GetAllMusicAsync();

        return musics.Where(m => m.artistId == artistId).Select(music => new MusicDTO
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
        }).ToList();

        /*var music = musics.FirstOrDefault(m => m.artistId == artistId);
        return music != null ? ConvertToDto(music) : null;*/
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
            var newMusicListen = new MusicListen
            {
                ListenDate = today,
                ListenCount = 1,
                MusicId = musicId
            };
            if (music.MusicListens == null) music.MusicListens = new List<MusicListen>();
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

    public async Task<bool> DeleteMusicByIdAsync(Guid musicId)
    {
        return await _musicRepository.DeleteMusicAsync(musicId);
    }

    public async Task<Music> GetMusicByMusicIdAsync(Guid musicId)
    {
        return await _musicRepository.GetMusicByIdAsync(musicId);
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

        /*var url = _s3Client.GetPreSignedURL(new GetPreSignedUrlRequest
        {
            BucketName = _bucketName,
            Key = filePath,
            Expires = DateTime.UtcNow.AddMinutes(30)
        });*/

        var aclResponse = await _s3Client.PutACLAsync(new PutACLRequest
        {
            BucketName = _bucketName,
            Key = filePath,
            CannedACL = S3CannedACL.PublicRead
        });

        var url = $"https://{_bucketName}.s3.amazonaws.com/{filePath}";
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