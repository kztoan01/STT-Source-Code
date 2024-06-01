using System;

namespace Sync.DTOs
{
    public class SongDTO
    {
        public Guid Id { get; set; }
        public string SongName { get; set; }
        public DateTime SongDate { get; set; }
        public Guid AlbumId { get; set; }
    }
}
