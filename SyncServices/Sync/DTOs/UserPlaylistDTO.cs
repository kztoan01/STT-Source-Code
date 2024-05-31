using System;

namespace Sync.DTOs
{
    public class UserPlaylistDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid PlaylistId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string PlaylistName { get; set; }
    }
}
