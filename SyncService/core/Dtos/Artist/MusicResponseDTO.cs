namespace core.Dtos.Artist;

public class MusicResponseDTO
{
    public Guid Id { get; set; }
    public string musicTitle { get; set; }
    public string musicUrl { get; set; }
    public string musicPicture { get; set; }
    public int musicPlays { get; set; }
    public string genreName { get; set; }
    public string albumTitle { get; set; }
}