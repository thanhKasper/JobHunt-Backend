using System.ComponentModel.DataAnnotations;

namespace JobHunt.Core.DTO;

public class RegisterDTO
{
    [Required(ErrorMessage = "Full Name is required.")]
    public string? Fullname { get; set; }

    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    [Required(ErrorMessage = "Email is required.")]
    [MaxLength(256, ErrorMessage = "Email cannot exceed 256 characters.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    public string? Password { get; set; } = null!;

    [Compare(nameof(Password),
        ErrorMessage = "The password and confirmation password do not match."
    )]
    [Required(ErrorMessage = "Confirm Password is required.")]
    public string? ConfirmPassword { get; set; }
}