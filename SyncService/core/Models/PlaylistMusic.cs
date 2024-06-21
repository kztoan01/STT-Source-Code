namespace core.Models;

public class PlaylistMusic
{
    public Guid playlistId { get; set; }
    public Guid musicId { get; set; }
    public DateTime addedAt { get; set; } = DateTime.Now;
    public int position { get; set; }
    public Music Music { get; set; }
    public Playlist Playlist { get; set; }
}