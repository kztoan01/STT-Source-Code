using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sync_service.Models
{
    public class Artist
    {
        public Guid Id { get; set; }
        public string userId { get; set; }
        public string artistDescription { get; set; } = string.Empty;
        public List<Album> Albums { get; set; } = new List<Album>();
        public List<Music> Musics { get; set; } = new List<Music>();
        public List<Follower> Followers { get; set; } = new List<Follower>();
        public User User { get; set; }
    }
}