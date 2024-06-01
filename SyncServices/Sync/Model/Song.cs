using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sync.Model
{
    public class Song
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(100)]
        public string SongName { get; set; }

        public DateTime SongDate { get; set; }

        public bool IsApproved { get; set; }

        public int RePlayTimes { get; set; }

        [ForeignKey("Album")]
        public Guid AlbumId { get; set; }
        public virtual Album Album { get; set; }

        public virtual ICollection<SongGenre> SongGenres { get; set; }
    }
}
