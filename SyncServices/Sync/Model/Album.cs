using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sync.Model
{
    public class Album
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(100)]
        public string Name { get; set; }

        public DateTime ReleaseDate { get; set; }

        [ForeignKey("Artist")]
        public Guid ArtistId { get; set; }
        public virtual Artist Artist { get; set; }
    }
}
