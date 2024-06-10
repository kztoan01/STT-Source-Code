using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sync_service.Models
{
    public class Genre
    {
        public Guid Id { get; set; }
        public string genreName { get; set; } = string.Empty;
        public string genreDescription { get; set; } = string.Empty;
        public List<Music> Musics { get; set; } = new List<Music>();
    }
}