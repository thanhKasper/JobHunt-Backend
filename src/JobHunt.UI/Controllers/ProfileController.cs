using JobHunt.Core.Domain.Entities;
using JobHunt.Core.DTO;
using JobHunt.Core.ServiceContracts;
using JobHunt.Ui.CustomModelBinders;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JobHunt.UI.Controllers;

public class ProfileController : ApiControllerBase
{
    private readonly ILogger<ProfileController> _logger;
    private readonly IProfileService _profileService;
    private readonly UserManager<JobHunter> _userManager;

    public ProfileController(
        ILogger<ProfileController> logger,
        IProfileService profileService,
        UserManager<JobHunter> userManager)
    {
        _profileService = profileService;
        _logger = logger;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<ActionResult<ProfileResponse>> Index(
        [ModelBinder(BinderType = typeof(UserIdBinder))] Guid userId)
    {
        var profile = await _profileService.GetProfileAsync(userId);
        return profile;
    }

    [HttpPut]
    public async Task<ActionResult<ProfileResponse>> Update(
        [ModelBinder(BinderType = typeof(UserIdBinder))] Guid userId,
        [FromBody] ProfileRequest profileRequest)
    {
        profileRequest.JobFinderId = userId;
        var updatedProfile = await _profileService.UpdateProfileAsync(profileRequest);
        return updatedProfile;
    }

    [HttpGet("simple-profile")]
    public async Task<ActionResult<ProfileSimpleResponse>> GetSimpleProfile(
        [ModelBinder(BinderType = typeof(UserIdBinder))] Guid? userId)
    {
        _logger.LogInformation("JOBHUNT - Calling GetSimpleProifle action method");
        if (!userId.HasValue) return BadRequest("UserId cannot be empty!");
        var user = await _userManager.FindByIdAsync(userId.Value.ToString());
        if (user is null) return BadRequest("No user found.");
        ProfileSimpleResponse profileSimple = new()
        {
            UserId = user.Id.ToString(),
            FullName = user.FullName,
            Email = user.Email
        };
        return profileSimple;
    }
}