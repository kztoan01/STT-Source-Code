namespace core.Models;

public class Follower
{
    public string userId { get; set; }
    public Guid artistId { get; set; }
    public DateTime beginDate { get; set; }
    public virtual User User { get; set; }
    public virtual Artist Artist { get; set; }
}