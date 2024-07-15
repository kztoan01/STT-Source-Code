using System.ComponentModel.DataAnnotations;

namespace core.Objects.Enum;

public enum ArtistSearchByEnum
{
    [Display(Name = "id")]
    Id,

    [Display(Name = "userId")]
    UserId,

    [Display(Name = "aristName")]
    AristName,

    [Display(Name = "artistDescription")]
    ArtistDescription,
    
    [Display(Name = "numberOfFollower")]
    NumberOfFollower
}