using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sync_service.Models
{
    public class Playlist
    {
        public Guid Id { get; set; }
        public string playlistName { get; set; } = string.Empty;
        public string playlistDescription { get; set; } = string.Empty;
        public string playlistPicture { get; set; } = string.Empty;
        public DateTime createdDate { get; set; } = DateTime.Now;
        public DateTime updatedDate { get; set; } = DateTime.Now;
        public string userId { get; set; }
        public User User { get; set; }
        public List<PlaylistMusic> playlistMusics { get; set; } = new List<PlaylistMusic>();
    }
}