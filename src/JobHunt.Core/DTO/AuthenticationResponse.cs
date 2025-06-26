namespace JobHunt.Core.DTO;

public class AuthenticationResponse
{
    public string? Token { get; set; }
    public DateTime? Expiration { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? UserId { get; set; }
}