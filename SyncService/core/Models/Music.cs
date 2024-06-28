namespace core.Models;

public class Music
{
    public Guid Id { get; set; }
    public string musicTitle { get; set; } = string.Empty;
    public string musicUrl { get; set; } = string.Empty;
    public string musicPicture { get; set; } = string.Empty;
    public int musicPlays { get; set; }
    public double musicDuration { get; set; }
    public DateTime releaseDate { get; set; }
    public Guid albumId { get; set; }
    public Album Album { get; set; }
    public Guid artistId { get; set; }
    public Artist Artist { get; set; }
    public Guid genreId { get; set; }
    public Genre Genre { get; set; }
    public List<PlaylistMusic> playlistMusics { get; set; } = new();

    public List<MusicListen> MusicListens { get; set; } = new();

    public List<Collaboration> collaborations { get; set; } = new();
}