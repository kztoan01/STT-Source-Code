using System.ComponentModel.DataAnnotations;

namespace Sync.Model
{
    public class Playlist
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public string PlaylistName { get; set; }

        [MaxLength(255)]
        public string PlaylistDescription { get; set; }

        public string PlaylistPicture { get; set; }

        public DateTime CreationDay { get; set; }

        public virtual ICollection<UserPlaylist> UserPlaylists { get; set; }
    }
}
