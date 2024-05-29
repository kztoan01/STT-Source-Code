using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sync.Model
{
    public class SongGenre
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [ForeignKey("Song")]
        public Guid SongID { get; set; }
        public virtual Song Song { get; set; }

        [ForeignKey("Genre")]
        public int GenreID { get; set; }
        public virtual Genre Genre { get; set; }
    }
}
