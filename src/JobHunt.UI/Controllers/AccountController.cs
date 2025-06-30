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
    IJwtService jwtService,
    ILogger<AccountController> logger) : ApiControllerBase
{
    private readonly UserManager<JobHunter> _userManager = userManager;
    private readonly SignInManager<JobHunter> _signInManager = signinManager;
    private readonly IJwtService _jwtService = jwtService;
    private readonly ILogger<AccountController> _logger = logger;

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
        _logger.LogInformation("JOBHUNT - Calling Login action method");
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

    /// <summary>
    /// Update the access token base on the refresh token provided by the user
    /// </summary>
    /// <param name="tokenModel">An object containing access token and refresh token</param>
    /// <returns>
    /// <para>Return the containing all necessary information</para>
    /// </returns>
    /// <exception cref="">Throw user-defined 452 error code indicating that user should re-login again</exception
    [HttpPost("revoke-token")]
    public async Task<IActionResult> RevokeToken([FromBody] TokenModel tokenModel)
    {
        _logger.LogInformation("JOBHUNT - Calling RevokeToken Action Method");
        try
        {
            var claimsPrinciple = _jwtService.GetPrincipalsFromJWT(tokenModel.AccessToken);
            if (claimsPrinciple is null) return BadRequest("Invalid jwt token");
            string? userId = claimsPrinciple.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId is null) return Problem("Invalid jwt format", statusCode: 452);

            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                return Problem("Expired Token due to user not found, please sign in again", statusCode: 452);
            }
            else if ((user.RefreshTokenExpirationDateTime ?? DateTime.UtcNow) < DateTime.UtcNow)
            {
                return Problem("Expired Token because refresh token is expired, please sign in again", statusCode: 452);
            }
            else if (user.RefreshToken != tokenModel.RefreshToken)
            {
                return Problem("Expired Token, refresh token not match, please sign in again", statusCode: 452);
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
            return Problem("Invalid Token Format. Please Check Again", statusCode: 452);
        }
    }
}