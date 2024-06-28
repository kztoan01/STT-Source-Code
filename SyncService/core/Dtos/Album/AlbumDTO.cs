namespace core.Dtos.Album;

public class AlbumDTO
{
    public Guid Id { get; set; }
    public string albumTitle { get; set; }
    public string albumDescription { get; set; } = string.Empty;
    public DateTime releaseDate { get; set; }
}