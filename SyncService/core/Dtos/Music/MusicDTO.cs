using core.Dtos.Album;

namespace core.Dtos.Music;

public class MusicDTO
{
    public Guid Id { get; set; }
    public string musicTitle { get; set; } = string.Empty;
    public string musicUrl { get; set; } = string.Empty;
    public string musicPicture { get; set; } = string.Empty;
    public int musicPlays { get; set; }
    public double musicDuration { get; set; }
    public DateTime releaseDate { get; set; }
    public string genreName { get; set; }

    public string artistName { get; set; }

    public AlbumDTO AlbumDTO { get; set; }
}