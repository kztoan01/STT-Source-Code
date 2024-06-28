using System.ComponentModel.DataAnnotations;

namespace core.Dtos.Account;

public class RegisterDTO
{
    [Required] public string? Username { get; set; }

    [Required] [EmailAddress] public string? Email { get; set; }

    [Required] public string? Password { get; set; }
}