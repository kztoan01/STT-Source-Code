﻿namespace core.Dtos.Album;

public class CreateAlbumDTO
{
    public string albumTitle { get; set; }
    public string albumDescription { get; set; }
    public DateTime releaseDate { get; set; }
}