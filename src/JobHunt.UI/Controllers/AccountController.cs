using JobHunt.Core.Domain.Entities;
using JobHunt.Core.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JobHunt.UI.Controllers;

public class AccountController(UserManager<JobHunter> userManager) : ApiControllerBase
{
    private readonly UserManager<JobHunter> _userManager = userManager;
    public async Task<IActionResult> Register(RegisterDTO registrationForm)
    {
        JobHunter user = new()
        {
            Email = registrationForm.Email,
            FullName = registrationForm.Fullname,
        };
        user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, registrationForm.Password!);

        IdentityResult result = await _userManager.CreateAsync(user);

        if (result.Succeeded)
        {
            return Ok();
        }
        else
        {
            IEnumerable<string> errorList = result.Errors.Select(e => e.Description);
            return BadRequest(new
            {
                Message = "Registration failed",
                Errors = errorList
            });
        }
    }
}