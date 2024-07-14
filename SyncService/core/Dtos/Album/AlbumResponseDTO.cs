using core.Dtos.Artist;
using core.Dtos.Music;
using core.Models;

namespace core.Dtos.Album;

public class AlbumResponseDTO
{
    public Guid Id { get; set; }
    public string albumTitle { get; set; }
    public string albumDescription { get; set; }
    public DateTime releaseDate { get; set; }

    public ArtistDTO? artist { get; set; }
    public List<MusicDTO> musics { get; set; }
    public string AlbumPicture { get; set; }
}