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
            await _signInManager.SignInAsync(user, isPersistent: false);
            var authenticationResponse = _jwtService.GenerateToken(user);
            return authenticationResponse;
        }
        else
        {
            IEnumerable<string> errorList = result.Errors.Select(e => e.Description);
            // return Problem(result.Errors.Select(e => e.Description).FirstOrDefault("Registration failed"));
            return Problem(errorList.FirstOrDefault());
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
                return Ok(authenRes);
            }
        }

        return Problem("Invalid login attempt. Please check your email and password.");
    }
}