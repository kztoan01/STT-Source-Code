using core.Dtos.Album;
using core.Dtos.Music;

namespace core.Dtos.Artist;

public class ArtistDTO
{
    public Guid Id { get; set; }
    public string userId { get; set; }
    public string ArtistName { get; set; }
    public string artistDescription { get; set; }
    public List<AlbumDTO> Albums { get; set; }
    public List<MusicDTO> ViralMusics { get; set; }
    public int NumberOfFollower { get; set; }
    public string artistImage { get; set; }
}