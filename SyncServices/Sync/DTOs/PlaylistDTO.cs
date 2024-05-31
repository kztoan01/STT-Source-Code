using System;

namespace Sync.DTOs
{
    public class PlaylistDTO
    {
        public Guid Id { get; set; }
        public string PlaylistName { get; set; }
        public string PlaylistDescription { get; set; }
        public string PlaylistPicture { get; set; }
        public DateTime CreationDay { get; set; }
    }
}
