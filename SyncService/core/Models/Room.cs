namespace core.Models;

public class Room
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public User User { get; set; }

    public string Code { get; set; }

    public string Image { get; set; }
    public string HostId { get; set; }

    public List<RoomPlaylist> RoomPlaylists { get; set; }

    public List<Participant> Participants { get; set; }
}