using System.ComponentModel.DataAnnotations;

namespace Sync.Model
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<SongGenre> SongGenres { get; set; }
    }
}
