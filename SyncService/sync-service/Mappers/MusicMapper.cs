using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sync_service.Dtos.Album;
using sync_service.Dtos.Music;
using sync_service.Models;

namespace sync_service.Mappers
{
    public static class MusicMapper
    {
         public static Music ToMusicFromCreate (this AddMusicDTO musicDTO)
        {
            return new Music
            {
                musicTitle = musicDTO.musicTitle,
                musicPlays = musicDTO.musicPlays,
                musicDuration = musicDTO.musicDuration,
                albumId = musicDTO.albumId,
                artistId = musicDTO.artistId,
                genreId = musicDTO.genreId,
            };
        }

        public static MusicDTO Convert(Music music)
        {
            if (music == null)
            {
                return null;
            }

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
                AlbumDTO = music.Album != null ? new AlbumDTO
                {
                    Id = music.Album.Id,
                    albumTitle = music.Album.albumTitle
                } : null,
                artistName = music.Artist?.User?.userFullName ?? string.Empty,
            };
        }
    }
}