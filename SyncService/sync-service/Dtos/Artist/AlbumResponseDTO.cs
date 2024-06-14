namespace sync_service.Dtos.Artist
{
    public class AlbumResponseDTO
    {
        public Guid Id { get; set; }
        public string albumTitle { get; set; }
        public string  albumDescription { get; set; }
        public DateTime releaseDate { get; set; }
    }
}
