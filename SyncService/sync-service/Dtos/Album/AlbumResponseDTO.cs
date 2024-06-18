using sync_service.Dtos.Artist;
using sync_service.Models;


namespace sync_service.Dtos.Album
{
    public class AlbumResponseDTO
    {
        public Guid Id { get; set; }
        public string albumTitle { get; set; }
        public string albumDescription { get; set; }
        public DateTime releaseDate { get; set; }

        public ArtistDTO? artist { get; set; }
    }
}
