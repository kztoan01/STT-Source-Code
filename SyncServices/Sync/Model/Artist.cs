using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sync.Model
{
    public class Artist
    {
        [Key]
        public Guid ArtistID { get; set; } = Guid.NewGuid();

        [MaxLength(100)]
        public string ArtistFirstName { get; set; }

        [MaxLength(100)]
        public string ArtistLastName { get; set; }

        public string ArtistDescription { get; set; }

        public DateTime ArtistBirthDate { get; set; }

        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}
