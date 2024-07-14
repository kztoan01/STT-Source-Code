

namespace core.Models;

public class Artist
{
    public Guid Id { get; set; }
    public string userId { get; set; }
    public string artistDescription { get; set; } = string.Empty;
    public List<Album> Albums { get; set; } = new();
    public List<Music> Musics { get; set; } = new();
    public List<Follower> Followers { get; set; } = new();
    public User User { get; set; }
    public string ImageUrl { get; set; }
}