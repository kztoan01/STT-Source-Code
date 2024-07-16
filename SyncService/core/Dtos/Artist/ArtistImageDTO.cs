using Microsoft.AspNetCore.Http;

namespace core.Dtos.Artist;

public class ArtistImageDTO
{
    public string description { get; set; }
    public IFormFile image { get; set; }
}