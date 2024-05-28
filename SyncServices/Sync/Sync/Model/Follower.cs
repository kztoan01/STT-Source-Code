using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sync.Model
{
    public class Follower
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("Artist")]
        public Guid ArtistId { get; set; }
        public virtual Artist Artist { get; set; }

        public DateTime BeginDate { get; set; }
    }
}
