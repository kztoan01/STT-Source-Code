namespace core.Models;

public class Follower
{
    public string userId { get; set; }
    public Guid artistId { get; set; }
    public DateTime beginDate { get; set; }
    public User User { get; set; }
    public Artist Artist { get; set; }
}