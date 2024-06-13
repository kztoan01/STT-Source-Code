using sync_service.Dtos.Album;
using sync_service.Dtos.Music;
using sync_service.Models;

namespace sync_service.Dtos.Artist
{
    public class ArtistDTO
    {
        public Guid Id { get; set; }
        public string userId { get; set; }
        public string AristName { get; set; }
        public string artistDescription { get; set; }
        public List<AlbumDTO> Albums { get; set; } 
        public List<MusicDTO> ViralMusics { get; set; } 
        public int NumberOfFollower { get; set; }
    }
}
