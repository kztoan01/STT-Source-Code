using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using core.Dtos.Album;
using core.Dtos.Music;
using core.Models;
using data.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using repository.Mappers;
using repository.Repository.Interfaces;

namespace repository.Repository;

public class MusicRepository : IMusicRepository
{
    private readonly string _bucketName = "sync-music-storage";
    private readonly ApplicationDBContext _context;
    private readonly IAmazonS3 _s3Client;

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
                artistName = m.Artist.User.userFullName
            })
            .ToListAsync();
    }

    public async Task<MusicDTO> GetMusicById(Guid id)
    {
        var music = await _context.Musics
            .Include(m => m.Album)
            .Include(m => m.Artist)
            .Include(m => m.Genre)
            .Include(m => m.Artist.User)
            .FirstOrDefaultAsync(m => m.Id.Equals(id));

        if (music == null) return null;

        var musicDTO = MusicMapper.Convert(music);

        return musicDTO;
    }

    public async Task<MusicDTO> GetMusicByArtistId(Guid id)
    {
        var music = await _context.Musics
            .Include(m => m.Album)
            .Include(m => m.Artist)
            .Include(m => m.Genre)
            .Include(m => m.Artist.User)
            .FirstOrDefaultAsync(m => m.artistId.Equals(id));

        if (music == null) return null;

        var musicDTO = MusicMapper.Convert(music);

        return musicDTO;
    }

    public async Task<string> Add1ListenTimeWhenMusicIsListened(Guid musicId)
    {
        var music = await _context.Musics
            .FirstOrDefaultAsync(m => m.Id == musicId);

        if (music == null) return null;

        music.musicPlays++;

        var today = DateTime.UtcNow.Date;
        var musicListen = await _context.MusicListens
            .FirstOrDefaultAsync(ml => ml.MusicId == musicId && ml.ListenDate == today);

        if (musicListen == null)
        {
            musicListen = new MusicListen
            {
                Id = Guid.NewGuid(),
                MusicId = musicId,
                ListenDate = today,
                ListenCount = 1
            };
            _context.MusicListens.Add(musicListen);
        }
        else
        {
            musicListen.ListenCount++;
            _context.MusicListens.Update(musicListen);
        }

        await _context.SaveChangesAsync();

        return "Added";
    }


    public async Task<int> ListenTimeOnThisDay(Guid musicId)
    {
        var today = DateTime.UtcNow.Date;
        var listenCount = await _context.MusicListens
            .Where(ml => ml.MusicId == musicId && ml.ListenDate == today)
            .SumAsync(ml => (int?)ml.ListenCount) ?? 0;

        return listenCount;
    }

    public async Task<int> ListenTimeOnThisMonth(Guid musicId)
    {
        var firstDayOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
        var listenCount = await _context.MusicListens
            .Where(ml => ml.MusicId == musicId && ml.ListenDate >= firstDayOfMonth)
            .SumAsync(ml => (int?)ml.ListenCount) ?? 0;

        return listenCount;
    }

    public async Task<int> ListenTimeOnThisYear(Guid musicId)
    {
        var firstDayOfYear = new DateTime(DateTime.UtcNow.Year, 1, 1);
        var listenCount = await _context.MusicListens
            .Where(ml => ml.MusicId == musicId && ml.ListenDate >= firstDayOfYear)
            .SumAsync(ml => (int?)ml.ListenCount) ?? 0;

        return listenCount;
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
}