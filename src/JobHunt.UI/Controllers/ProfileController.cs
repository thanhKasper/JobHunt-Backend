using JobHunt.Core.DTO;
using JobHunt.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace JobHunt.UI.Controllers;

public class ProfileController : ApiControllerBase
{
    private readonly ILogger<ProfileController> _logger;
    private readonly IProfileService _profileService;

    public ProfileController(ILogger<ProfileController> logger, IProfileService profileService)
    {
        _profileService = profileService;
        _logger = logger;
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<ProfileResponse>> Index(Guid userId)
    {
        var profile = await _profileService.GetProfileAsync(userId);

        return profile;
    }

    [HttpPut("{userId}")]
    public async Task<ActionResult<ProfileResponse>> Update(Guid userId, [FromBody] ProfileRequest profileRequest)
    {
        var updatedProfile = await _profileService.UpdateProfileAsync(profileRequest);
        return updatedProfile;
    }
}