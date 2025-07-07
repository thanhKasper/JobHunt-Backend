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
    private readonly IMajorService _majorService;
    private readonly IEducationService _educationService;

    public ProfileController(
        ILogger<ProfileController> logger,
        IProfileService profileService,
        IMajorService majorService,
        IEducationService educationService,
        UserManager<JobHunter> userManager)
    {
        _profileService = profileService;
        _logger = logger;
        _userManager = userManager;
        _majorService = majorService;
        _educationService = educationService;
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

    [HttpGet("major-list")]
    public async Task<ActionResult<List<MajorKeyValuePair>>> GetMajorList()
    {
        var majors = await _majorService.GetMajorListAsync();
        return majors;
    }

    [HttpGet("degree-list")]
    public async Task<ActionResult<List<EducationKeyValuePair>>> GetDegreeList()
    {
        var degrees = await _educationService.GetDegreeListAsync();
        return degrees;
    }
}