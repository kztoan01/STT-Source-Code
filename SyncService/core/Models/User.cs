using Microsoft.AspNetCore.Identity;

namespace core.Models;

public class User : IdentityUser
{
    public string userFullName { get; set; } = string.Empty;
    public DateTime birthday { get; set; }
    public string address { get; set; } = string.Empty;
    public string city { get; set; } = string.Empty;
    public bool status { get; set; }
    public List<Playlist> Playlists { get; set; } = new();
    public List<Follower> Followers { get; set; } = new();

    public List<MusicHistory> MusicHistories  { get; set; } = new();
    public Artist Artist { get; set; }

    public Room Room { get; set; } 
    public List<Participant> Participants { get; set; } = new();
    //public AccountDetail AccountDetail { get; set; }

}