using Microsoft.AspNetCore.Http;

namespace core.Dtos.Music;

public class AddMusicDTO
{
    public IFormFile fileMusic { get; set; }
    public IFormFile fileImage { get; set; }
    public string musicTitle { get; set; } = string.Empty;
    public int musicPlays { get; set; }
    public double musicDuration { get; set; }
    public Guid albumId { get; set; }
    public Guid artistId { get; set; }
    public Guid genreId { get; set; }
}