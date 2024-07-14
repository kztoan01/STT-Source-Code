using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using core.Dtos.Album;
using core.Dtos.Music;
using core.Dtos.Playlist;
using core.Models;
using core.Objects;
using Microsoft.AspNetCore.Http;
using repository.Repository.Interfaces;
using service.Service.Interfaces;

namespace service.Service;

public class PlaylistService : IPlaylistService
{
    private readonly IAlbumRepository _albumRepository;
    private readonly IMusicRepository _musicRepository;
    private readonly IPlaylistRepository _playlistRepository;
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName = "sync-music-storage";

    public PlaylistService(IPlaylistRepository playlistRepository, IMusicRepository musicRepository,IAlbumRepository albumRepository, IAmazonS3 amazonS3)
    {
        _playlistRepository = playlistRepository;
        _musicRepository = musicRepository;
        _albumRepository = albumRepository;
        _s3Client = amazonS3;
    }

    public async Task<Playlist> CreatePlaylistAsync(Playlist playlist, IFormFile image)
    {
        var imageUrl = await UploadFileAsync(image);
        playlist.ImageUrl = imageUrl;
        return await _playlistRepository.CreatePlaylistAsync(playlist);
    }

    public async Task<Playlist?> DeletePlaylistAsync(Guid id)
    {
        return await _playlistRepository.DeletePlaylistAsync(id);
    }

    public async Task<PlaylistDTO> GetPlaylistByIdAsync(Guid id)
    {
        var playlist = await _playlistRepository.GetPlaylistByIdAsync(id);
        var playlistDTO = new PlaylistDTO
        {
            Id = playlist.Id,
            createdDate = playlist.createdDate,
            playlistDescription = playlist.playlistDescription,
            playlistName = playlist.playlistName,
            playlistPicture = playlist.playlistPicture,
            userId = playlist.userId,
            updatedDate = playlist.updatedDate,
            musics = new List<MusicDTO>()
        };

        var musicsList = await _musicRepository.GetMusicInPlaylistByPlaylsitIdAsync(playlist.Id);
        foreach (var music in musicsList)
        {
            var existMusic = await _musicRepository.GetMusicByIdAsync(music.musicId);
            if (existMusic == null || existMusic.Id == Guid.Empty) throw new Exception("Music is null or not found");

            var artistName = existMusic.Artist?.User?.UserName ?? "No artist available";
            var genreName = existMusic.Genre?.genreName ?? "No genre available";

            playlistDTO.musics.Add(new MusicDTO
            {
                artistName = artistName,
                genreName = genreName,
                musicDuration = existMusic.musicDuration,
                musicPicture = existMusic.musicPicture,
                musicPlays = existMusic.musicPlays,
                musicTitle = existMusic.musicTitle,
                musicUrl = existMusic.musicUrl,
                releaseDate = existMusic.releaseDate,
                Id = existMusic.Id
            });
        }

        return playlistDTO;
    }


    public async Task<List<PlaylistDTO>> GetUserPlaylistsAsync(string userId, QueryObject query)
    {
        var playlists = await _playlistRepository.GetUserPlaylistsAsync(userId);

        if (!string.IsNullOrWhiteSpace(query.SortBy))
            playlists = query.SortBy.Equals("Name", StringComparison.OrdinalIgnoreCase)
                ? query.IsDecsending
                    ? playlists.OrderByDescending(p => p.playlistName).ToList()
                    : playlists.OrderBy(p => p.playlistName).ToList()
                : playlists;

        var skipNumber = (query.PageNumber - 1) * query.PageSize;
        var paginatedPlaylists = playlists.Skip(skipNumber).Take(query.PageSize).ToList();

        var playlistDTOs = new List<PlaylistDTO>();

        foreach (var playlist in paginatedPlaylists)
        {
            var playlistDTO = new PlaylistDTO
            {
                Id = playlist.Id,
                createdDate = playlist.createdDate,
                playlistDescription = playlist.playlistDescription,
                playlistName = playlist.playlistName,
                playlistPicture = playlist.playlistPicture,
                userId = playlist.userId,
                updatedDate = playlist.updatedDate,
                musics = new List<MusicDTO>()
            };

            var musicsList = await _musicRepository.GetMusicInPlaylistByPlaylsitIdAsync(playlist.Id);
            foreach (var music in musicsList)
            {
                var existMusic = await _musicRepository.GetMusicByIdAsync(music.musicId);
                if (existMusic == null)
                    throw new Exception("Music is null or not found");

                playlistDTO.musics.Add(new MusicDTO
                {
                    artistName = existMusic.Artist?.User?.UserName ?? "No artist available",
                    genreName = existMusic.Genre?.genreName ?? "No genre available",
                    musicDuration = existMusic.musicDuration,
                    musicPicture = existMusic.musicPicture,
                    musicPlays = existMusic.musicPlays,
                    musicTitle = existMusic.musicTitle,
                    musicUrl = existMusic.musicUrl,
                    releaseDate = existMusic.releaseDate,
                    Id = existMusic.Id
                });
            }

            playlistDTOs.Add(playlistDTO);
        }

        return playlistDTOs;
    }


    public async Task<List<PlaylistDTO>> GetPlaylistsByGenreNameAsync(string genreName, QueryObject query)
    {
        var playlists = await _playlistRepository.GetPlaylistsByGenreNameAsync(genreName);

        if (!string.IsNullOrWhiteSpace(query.SortBy))
            playlists = query.SortBy.Equals("Name", StringComparison.OrdinalIgnoreCase)
                ? query.IsDecsending
                    ? playlists.OrderByDescending(p => p.playlistName).ToList()
                    : playlists.OrderBy(p => p.playlistName).ToList()
                : playlists;

        var skipNumber = (query.PageNumber - 1) * query.PageSize;
        var paginatedPlaylists = playlists.Skip(skipNumber).Take(query.PageSize).ToList();

        var playlistDTOs = new List<PlaylistDTO>();

        foreach (var playlist in paginatedPlaylists)
        {
            var playlistDTO = new PlaylistDTO
            {
                Id = playlist.Id,
                createdDate = playlist.createdDate,
                playlistDescription = playlist.playlistDescription,
                playlistName = playlist.playlistName,
                playlistPicture = playlist.playlistPicture,
                userId = playlist.userId,
                updatedDate = playlist.updatedDate,
                musics = new List<MusicDTO>()
            };

            var musicsList = await _musicRepository.GetMusicInPlaylistByPlaylsitIdAsync(playlist.Id);
            foreach (var music in musicsList)
            {
                var existMusic = await _musicRepository.GetMusicByIdAsync(music.musicId);
                if (existMusic == null)
                    throw new Exception("Music is null or not found");

                playlistDTO.musics.Add(new MusicDTO
                {
                    artistName = existMusic.Artist?.User?.UserName ?? "No artist available",
                    genreName = existMusic.Genre?.genreName ?? "No genre available",
                    musicDuration = existMusic.musicDuration,
                    musicPicture = existMusic.musicPicture,
                    musicPlays = existMusic.musicPlays,
                    musicTitle = existMusic.musicTitle,
                    musicUrl = existMusic.musicUrl,
                    releaseDate = existMusic.releaseDate,
                    Id = existMusic.Id
                });
            }

            playlistDTOs.Add(playlistDTO);
        }

        return playlistDTOs;
    }


    public async Task<List<PlaylistDTO>> ShowPlaylistsByUserIdAsync(Guid userId, QueryObject query)
    {
        var playlists = await _playlistRepository.ShowPlaylistsByUserIdAsync(userId);

        if (!string.IsNullOrWhiteSpace(query.SortBy))
            playlists = query.SortBy.Equals("Name", StringComparison.OrdinalIgnoreCase)
                ? query.IsDecsending
                    ? playlists.OrderByDescending(p => p.playlistName).ToList()
                    : playlists.OrderBy(p => p.playlistName).ToList()
                : playlists;

        var skipNumber = (query.PageNumber - 1) * query.PageSize;
        var paginatedPlaylists = playlists.Skip(skipNumber).Take(query.PageSize).ToList();

        var playlistDTOs = new List<PlaylistDTO>();

        foreach (var playlist in paginatedPlaylists)
        {
            var playlistDTO = new PlaylistDTO
            {
                Id = playlist.Id,
                createdDate = playlist.createdDate,
                playlistDescription = playlist.playlistDescription,
                playlistName = playlist.playlistName,
                playlistPicture = playlist.playlistPicture,
                userId = playlist.userId,
                updatedDate = playlist.updatedDate,
                musics = new List<MusicDTO>()
            };

            var musicsList = await _musicRepository.GetMusicInPlaylistByPlaylsitIdAsync(playlist.Id);
            foreach (var music in musicsList)
            {
                var existMusic = await _musicRepository.GetMusicByIdAsync(music.musicId);
                if (existMusic == null)
                    throw new Exception("Music is null or not found");

                playlistDTO.musics.Add(new MusicDTO
                {
                    artistName = existMusic.Artist?.User?.UserName ?? "No artist available",
                    genreName = existMusic.Genre?.genreName ?? "No genre available",
                    musicDuration = existMusic.musicDuration,
                    musicPicture = existMusic.musicPicture,
                    musicPlays = existMusic.musicPlays,
                    musicTitle = existMusic.musicTitle,
                    musicUrl = existMusic.musicUrl,
                    releaseDate = existMusic.releaseDate,
                    Id = existMusic.Id
                });
            }

            playlistDTOs.Add(playlistDTO);
        }

        return playlistDTOs;
    }


    public async Task<Playlist?> UpdatePlaylistAsync(Guid id, Playlist playlistModel)
    {
        return await _playlistRepository.UpdatePlaylistAsync(id, playlistModel);
    }

    public async Task<string> AddMusicIntoPlaylistAsync(Guid musicId, Guid playlistId)
    {
        var playlist = await _playlistRepository.GetPlaylistByIdAsync(playlistId);
        var music = await _musicRepository.GetMusicByIdAsync(musicId);

        if (playlist == null || music == null)
            return "Music or Playlist are not available!";

        if (playlist.playlistMusics.Any(pm => pm.musicId == musicId))
            return "This music is already added into this Playlist.";

        var nextPosition = playlist.playlistMusics.Count + 1;

        var playlistMusic = new PlaylistMusic
        {
            playlistId = playlistId,
            musicId = musicId,
            addedAt = DateTime.UtcNow,
            position = nextPosition,
            Music = music,
            Playlist = playlist
        };

        playlist.playlistMusics.Add(playlistMusic);
        await _playlistRepository.UpdatePlaylistAsync(playlistId, playlist);

        return "Added successfully!";
    }

    public async Task<string> AddEntireAlbumIntoPlaylistAsync(Guid albumId, Guid playlistId)
    {
        var musics = await _musicRepository.GetMusicByAlbumIdAsync(albumId);

        if (musics == null || musics.Count == 0)
            return "No music found in the specified album.";

        var playlist = await _playlistRepository.GetPlaylistByIdAsync(playlistId);

        if (playlist == null)
            return "Playlist not found.";

        var addedMusicTitles = new List<string>();
        var skippedMusicTitles = new List<string>();

        foreach (var music in musics)
        {
            var result = await AddMusicIntoPlaylistAsync(music.Id, playlistId);

            if (result.Equals("This music is already added into this Playlist."))
                skippedMusicTitles.Add(music.musicTitle);
            else
                addedMusicTitles.Add(music.musicTitle);
        }

        var message = "Album added to playlist successfully!";

        if (addedMusicTitles.Count > 0)
            message += " Added music: " + string.Join(";", addedMusicTitles);

        if (skippedMusicTitles.Count > 0)
            message += " Skipped music: " + string.Join(";", skippedMusicTitles);

        return message;
    }

    public async Task<List<AlbumDTO>> GetAlbumByContainArtistByArtistId(Guid artistId)
    {
        return await _albumRepository.GetAlbumByContainArtistByArtistId(artistId);
    }

    public async Task<string> DeleteMusicInPlaylistAsync(Guid musicId, Guid playlistId)
    {
        var playlist = await _playlistRepository.GetPlaylistByIdAsync(playlistId);

        if (playlist == null) return "Playlist is null!";

        var music = await _musicRepository.GetMusicByIdAsync(musicId);

        if (music == null) return "Music is null!";

        var playlistMusic = playlist.playlistMusics.FirstOrDefault(pm => pm.musicId == musicId);

        if (playlistMusic == null) return "Invalid";

        var deletedPosition = playlistMusic.position;

        playlist.playlistMusics.Remove(playlistMusic);

        foreach (var item in playlist.playlistMusics.Where(pm => pm.position > deletedPosition))
            item.position--;

        await _playlistRepository.UpdatePlaylistAsync(playlistId, playlist);
        return "Removed music from playlist successfully!";
    }


    public async Task<string> ChangeMusicPositionInPlaylistAsync(Guid musicId, int newPosition, Guid playlistId)
    {
        var playlist = await _playlistRepository.GetPlaylistByIdAsync(playlistId);

        if (playlist == null) return "Playlist not found!";

        var music = playlist.playlistMusics.FirstOrDefault(pm => pm.musicId.Equals(musicId));

        if (music == null) return "Music not found in the playlist.";

        var oldPosition = music.position;
        if (oldPosition == newPosition) return "New position is the same as old position.";

        var playlistMusics = playlist.playlistMusics.OrderBy(pm => pm.position).ToList();

        if (oldPosition > newPosition)
        {
            var changeList = playlistMusics
                .Where(pm => pm.position >= newPosition && pm.position < oldPosition)
                .ToList();
            foreach (var item in changeList) item.position++;
            music.position = newPosition;
        }
        else if (oldPosition < newPosition)
        {
            var changeList = playlistMusics
                .Where(pm => pm.position > oldPosition && pm.position <= newPosition)
                .ToList();
            foreach (var item in changeList) item.position--;
            music.position = newPosition;
        }

        await _playlistRepository.UpdatePlaylistAsync(playlistId, playlist);
        return "Music positions in the playlist have been updated successfully.";
    }

    private async Task<string> UploadFileAsync(IFormFile file)
    {
        var fileTransferUtility = new TransferUtility(_s3Client);
        var fileExtension = Path.GetExtension(file.FileName);
        var filePath = $"image/{Guid.NewGuid()}{fileExtension}";

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