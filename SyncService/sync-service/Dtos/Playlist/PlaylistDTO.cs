using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sync_service.Dtos.Playlist
{
    public class PlaylistDTO
    {
        public int Id { get; set; }
        public string playlistName { get; set; } = string.Empty;
        public string playlistDescription { get; set; } = string.Empty ;
        public string playlistPicture { get; set; } = string.Empty;
        public DateTime createdDate { get; set; } = DateTime.Now;
        public DateTime updatedDate { get; set; } = DateTime.Now;
        public int userId { get; set; }
    }
}