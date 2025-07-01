using System.ComponentModel.DataAnnotations;
using JobHunt.Core.Domain.Entities;
using JobHunt.Core.Domain.RepositoryContracts;
using JobHunt.Core.DTO;
using JobHunt.Core.ServiceContracts;

namespace JobHunt.Core.Services;

public class ProfileService(IProfileRepository profileRepository) : IProfileService
{
    private readonly IProfileRepository _profileRepository = profileRepository;

    public async Task<ProfileResponse> GetProfileAsync(Guid? jobHunterId)
    {
        if (jobHunterId == null)
        {
            throw new ArgumentException("Job hunter ID cannot be null.");
        }

        var jobHunter = await _profileRepository.GetProfileAsync(jobHunterId.Value)
         ?? throw new ArgumentException($"Profile with ID {jobHunterId} not found.");

        return jobHunter.ToProfileResponse();
    }

    public async Task<ProfileResponse> UpdateProfileAsync(ProfileRequest? profileRequest)
    {
        ArgumentNullException.ThrowIfNull(profileRequest);

        ValidationContext validationCtx = new(profileRequest);
        List<ValidationResult> validationResults = [];
        if (!Validator.TryValidateObject(profileRequest, validationCtx, validationResults, true))
        {
            throw new ArgumentException(validationResults.FirstOrDefault()?.ErrorMessage);
        }

        JobHunter? existingProfile = await _profileRepository.GetProfileAsync(profileRequest.JobFinderId);
        if (existingProfile == null)
        {
            throw new ArgumentException($"Profile with ID {profileRequest.JobFinderId} not found.");
        }
        // Update the existing profile with the new values from the request
        var jobHunter = profileRequest.ToJobHunter();
        var updatedJobHunter = await _profileRepository.UpdateProfileAsync(jobHunter);
        return updatedJobHunter.ToProfileResponse();
    }
}