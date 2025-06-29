using System.Security.Claims;
using JobHunt.Core.Domain.Entities;
using JobHunt.Core.DTO;
using JobHunt.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JobHunt.UI.Controllers;

[AllowAnonymous]
public class AccountController(
    UserManager<JobHunter> userManager,
    SignInManager<JobHunter> signinManager,
    IJwtService jwtService) : ApiControllerBase
{
    private readonly UserManager<JobHunter> _userManager = userManager;
    private readonly SignInManager<JobHunter> _signInManager = signinManager;
    private readonly IJwtService _jwtService = jwtService;

    [HttpPost("register")]
    public async Task<ActionResult<AuthenticationResponse>> Register(RegisterDTO registrationForm)
    {
        JobHunter user = new()
        {
            Email = registrationForm.Email,
            FullName = registrationForm.Fullname,
            UserName = registrationForm.Email, // Using email as username
        };

        IdentityResult result = await _userManager.CreateAsync(user, registrationForm.Password!);

        if (result.Succeeded)
        {
            var authenticationResponse = _jwtService.GenerateToken(user);
            user.RefreshToken = authenticationResponse.RefreshToken;
            user.RefreshTokenExpirationDateTime = authenticationResponse.RefreshTokenExpiration;
            await _signInManager.SignInAsync(user, isPersistent: false);
            return authenticationResponse;
        }
        else
        {
            IdentityError error = result.Errors.First();

            return ValidationProblem(
                title: error.Code,
                detail: error.Description,
                statusCode: 400
            );
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO login)
    {
        var user = await _userManager.FindByEmailAsync(login.Email!);
        if (user != null)
        {

            var result = await _signInManager.PasswordSignInAsync(
                login.Email!,
                login.Password!,
                isPersistent: true,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var authenRes = _jwtService.GenerateToken(user);
                user.RefreshToken = authenRes.RefreshToken;
                user.RefreshTokenExpirationDateTime = authenRes.RefreshTokenExpiration;
                await _userManager.UpdateAsync(user);
                return Ok(authenRes);
            }
        }

        return Problem("Invalid login attempt. Please check your email and password.");
    }

    [HttpPost("revoke-token")]
    public async Task<IActionResult> RevokeToken([FromBody] TokenModel tokenModel)
    {
        try
        {
            var claimsPrinciple = _jwtService.GetPrincipalsFromJWT(tokenModel.AccessToken);
            if (claimsPrinciple is null) return BadRequest("Invalid jwt token");
            string? userId = claimsPrinciple.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId is null) return BadRequest("Invalid jwt format");

            var user = await _userManager.FindByIdAsync(userId);

            if (user is null ||
                (user.RefreshTokenExpirationDateTime ?? DateTime.UtcNow)
                .ToUniversalTime() < DateTime.UtcNow ||
                user.RefreshToken != tokenModel.RefreshToken)
            {
                return BadRequest("Expired Token, please sign in again");
            }
            else
            {

                AuthenticationResponse newToken = _jwtService.GenerateToken(user);

                user.RefreshToken = newToken.RefreshToken;
                user.RefreshTokenExpirationDateTime = newToken.RefreshTokenExpiration;
                await _userManager.UpdateAsync(user);
                return Ok(newToken);

            }
        }
        catch
        {
            return BadRequest("Invalid Token Format. Please Check Again");
        }
    }
}