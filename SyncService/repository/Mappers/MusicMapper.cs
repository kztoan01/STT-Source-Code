using core.Dtos.Album;
using core.Dtos.Music;
using core.Models;

namespace repository.Mappers;

public static class MusicMapper
{
    public static ElasticMusicDTO ToElasticFromMusic(this Music music)
    {
        return new ElasticMusicDTO
        {
            Id = music.Id,
            musicTitle = music.musicTitle,
            musicUrl = music.musicUrl,
            musicPicture = music.musicPicture
        };
    }
    public static Music ToMusicFromCreate(this AddMusicDTO musicDTO)
    {
        return new Music
        {
            musicTitle = musicDTO.musicTitle,
            musicPlays = musicDTO.musicPlays,
            musicDuration = musicDTO.musicDuration,
            albumId = musicDTO.albumId,
            artistId = musicDTO.artistId,
            genreId = musicDTO.genreId
        };
    }

    public static MusicDTO Convert(Music music)
    {
        if (music == null) return null;

        return new MusicDTO
        {
            Id = music.Id,
            genreName = music.Genre?.genreName ?? string.Empty,
            musicDuration = music.musicDuration,
            musicPicture = music.musicPicture,
            musicPlays = music.musicPlays,
            musicTitle = music.musicTitle,
            musicUrl = music.musicUrl,
            releaseDate = music.releaseDate,
            AlbumDTO = music.Album != null
                ? new AlbumDTO
                {
                    Id = music.Album.Id,
                    albumTitle = music.Album.albumTitle
                }
                : null,
            artistName = music.Artist?.User?.userFullName ?? string.Empty
        };
    }
}