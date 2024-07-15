namespace core.Models;

public class Album
{
    public Guid Id { get; set; }
    public string albumTitle { get; set; } = string.Empty;
    public string albumDescription { get; set; } = string.Empty;
    public DateTime releaseDate { get; set; }
    public Guid artistId { get; set; }
    public List<Music> Musics { get; set; } = new();
    public string ImageUrl { get; set; }
}