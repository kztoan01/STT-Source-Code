using core.Models;

namespace core.Dtos.User;

public class UserDTO
{
    public UserDTO(){}
    public string userFullName { get; set; }
    public DateTime birthday { get; set; }
    public string address { get; set; }
    public string city { get; set; }
    public bool status { get; set; }
    public List<Models.Playlist> Playlists { get; set; }
    public List<Follower> Followers { get; set; }

    public List<MusicHistory> MusicHistories  { get; set; } 

    public UserDTO(Models.User user)
    {
        userFullName = user.userFullName;
        birthday = user.birthday;
        address = user.address;
        city = user.city;
        status = user.status;
        Playlists = user.Playlists;
        Followers = user.Followers;
        MusicHistories = user.MusicHistories;
    }
}