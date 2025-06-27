using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using JobHunt.Core.Domain.Entities;
using JobHunt.Core.DTO;
using JobHunt.Core.ServiceContracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.Name, user.FullName!),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
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
            UserId = user.Id.ToString(),
            RefreshToken = this.GenerateRefreshToken(),
            RefreshTokenExpiration = DateTime.UtcNow.AddMinutes(
                _configuration.GetSection("RefreshToken").GetValue<int>("Expiration_Minutes", 120)
            )
        };
    }

    private string GenerateRefreshToken()
    {
        byte[] randomBytes = new byte[64];
        var randomGenerator = RandomNumberGenerator.Create();
        randomGenerator.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    public ClaimsPrincipal? GetPrincipalsFromJWT(string? token)
    {
        ArgumentNullException.ThrowIfNull(token, "token cannot be empty");

        // How should the Jwt should check for validation?
        TokenValidationParameters tokenValidation = new()
        {
            ValidateIssuer = true,
            ValidIssuer = _configuration.GetValue<string>("Jwt:Issuer", ""),
            ValidateAudience = true,
            ValidAudience = _configuration.GetValue<string>("Jwt:Audience", ""),
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(
                    _configuration.GetValue("Jwt:Key", "")
                )
            ),
            ValidateLifetime = false
        };
        JwtSecurityTokenHandler jwtHandler = new();
        ClaimsPrincipal claimsPrincipal =
            jwtHandler.ValidateToken(token, tokenValidation, out SecurityToken secKey);

        if (secKey is not JwtSecurityToken jwtToken ||
            !jwtToken.Header.Alg.Contains(
                SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid Token");
        }

        return claimsPrincipal;
    }
}