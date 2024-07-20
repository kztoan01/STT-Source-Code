using System.ComponentModel.DataAnnotations;

namespace core.Objects.Enum;

public enum AlbumSearchByEnum
{
    [Display(Name = "AlbumTitle")] AlbumTitle,
    [Display(Name = "AlbumDescription")] AlbumDescription,
    [Display(Name = "ReleaseDate")] ReleaseDate
}