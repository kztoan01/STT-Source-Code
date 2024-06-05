using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sync_service.Models
{
    public class PlaylistMusic
    {
        public int playlistId { get; set; }
        public int musicId { get; set; }
        public DateTime addedAt { get; set; } = DateTime.Now;
        public int position { get; set; }
        public Music Music { get; set; }
        public Playlist Playlist { get; set; }
    }
}