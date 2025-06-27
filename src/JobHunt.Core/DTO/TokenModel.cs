using System.ComponentModel.DataAnnotations;

namespace JobHunt.Core.DTO;

public class TokenModel
{
    [Required]
    public string? AccessToken { get; set; } = string.Empty;
    public string? RefreshToken { get; set; } = string.Empty;
}