using System.ComponentModel.DataAnnotations;

namespace BlazorSimpleAuth.Models;

public class LoginUserModel
{
    [Required]
    public string? Username { get; set; }

    [Required]
    public string? Password { get; set; }
}
