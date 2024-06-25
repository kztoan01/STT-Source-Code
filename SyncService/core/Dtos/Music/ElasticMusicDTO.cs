namespace core.Dtos.Music;

public class ElasticMusicDTO
{
    public Guid Id { get; set; }
    public string musicTitle { get; set; } = string.Empty;
    public string musicUrl { get; set; } = string.Empty;
    public string musicPicture { get; set; } = string.Empty;
    
}