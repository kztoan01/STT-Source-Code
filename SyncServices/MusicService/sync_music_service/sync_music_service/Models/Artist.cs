using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sync_music_service.Models
{
    public class Artist
    {
        public int Id { get; set; }
        public int userId { get; set; }
        public int artistDescription { get; set; }
        public List<Album> Albums { get; set; } = new List<Album>();
        public List<Music> Musics { get; set; } = new List<Music>();
    }
}