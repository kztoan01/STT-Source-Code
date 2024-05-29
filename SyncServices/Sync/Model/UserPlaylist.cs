using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sync.Model
{
    public class UserPlaylist
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("Playlist")]
        public Guid PlaylistId { get; set; }
        public virtual Playlist Playlist { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
