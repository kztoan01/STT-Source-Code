using System;

namespace Sync.DTOs
{
    public class AlbumDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public Guid ArtistId { get; set; }
    }
}
