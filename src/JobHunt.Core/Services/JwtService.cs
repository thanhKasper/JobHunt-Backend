using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using JobHunt.Core.Domain.Entities;
using JobHunt.Core.DTO;
using JobHunt.Core.ServiceContracts;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JobHunt.Core.Services;

public class JwtService(IConfiguration configuration) : IJwtService
{
    private readonly IConfiguration _configuration = configuration;
    public AuthenticationResponse GenerateToken(JobHunter user)
    {
        DateTime expirationTime =
            DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Jwt:Expiration_Minutes"));

        string issuer = _configuration.GetValue<string>("Jwt:Issuer", "");
        string audience = _configuration.GetValue<string>("Jwt:Audience", "");


        // Payload of JWT token
        Claim[] claims =
        [
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Email!),
            new Claim(ClaimTypes.Name, user.FullName!),
        ];

        // Generate secret key
        string Key = _configuration.GetValue<string>("Jwt:Key", "");
        SecurityKey secKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Key));
        SigningCredentials signingCredentials = new(secKey, SecurityAlgorithms.HmacSha256);
        JwtSecurityToken jwtToken = new(
            issuer,
            audience,
            claims,
            expires: expirationTime,
            signingCredentials: signingCredentials
        );

        JwtSecurityTokenHandler jwtTokenHandler = new();
        string token = jwtTokenHandler.WriteToken(jwtToken);
        return new AuthenticationResponse
        {
            Token = token,
            Expiration = expirationTime,
            FullName = user.FullName,
            Email = user.Email,
            UserId = user.Id.ToString()
        };
    }
}