using core.Dtos.Music;

namespace core.Dtos.Playlist;

public class PlaylistDTO
{
    public Guid Id { get; set; }
    public string playlistName { get; set; } = string.Empty;
    public string playlistDescription { get; set; } = string.Empty;
    public string playlistPicture { get; set; } = string.Empty;
    public DateTime createdDate { get; set; } = DateTime.Now;
    public DateTime updatedDate { get; set; } = DateTime.Now;
    public string userId { get; set; }

    public List<MusicDTO> musics { get; set; }
}