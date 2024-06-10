using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace sync_service.Models
{
    public class User : IdentityUser
    {
        public string userFullName { get; set; } = string.Empty;   
        public DateTime birthday { get; set; }
        public string address { get; set; } = string.Empty;
        public string city { get; set; } = string.Empty;
        public bool status { get; set; }
        public List<Playlist> Playlists { get; set; } = new List<Playlist>();
        public List<Follower> Followers { get; set; } = new List<Follower>();
        public Artist Artist { get; set; }
    }
}