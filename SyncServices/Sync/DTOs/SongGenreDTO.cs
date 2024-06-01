using System;

namespace Sync.DTOs
{
    public class SongGenreDTO
    {
        public Guid Id { get; set; }
        public Guid SongID { get; set; }
        public int GenreID { get; set; }
    }
}
