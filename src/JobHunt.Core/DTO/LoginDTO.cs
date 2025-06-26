using System.ComponentModel.DataAnnotations;

namespace JobHunt.Core.DTO;

public class LoginDTO
{
    [Required(ErrorMessage = "Email is required")]
    public string? Email { get; set; }
    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }
}