using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                musicDuration = musicDTO.musicDuration
            };
        }
    }
}