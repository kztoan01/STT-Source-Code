using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace core.Dtos.Playlist;

public class CreatePlaylistDTO
{
    [Required]
    [MinLength(5, ErrorMessage = "Title must be 5 characters")]
    [MaxLength(280, ErrorMessage = "Title cannot be over 280 characters")]
    public string playlistName { get; set; } = string.Empty;

    public string playlistDescription { get; set; } = string.Empty;
    public IFormFile playlistPicture { get; set; }
}