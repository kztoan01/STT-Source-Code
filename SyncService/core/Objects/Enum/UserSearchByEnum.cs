using System.ComponentModel.DataAnnotations;

namespace core.Objects.Enum;

public enum UserSearchByEnum
{
    [Display(Name = "userFullName")] userFullName,
    [Display(Name = "birthday")] birthday,
    [Display(Name = "address")] address,
    [Display(Name = "city")] city,
    [Display(Name = "status")] status
}