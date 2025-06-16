using JobHunt.Core.DTO;

namespace JobHunt.Core.ServiceContracts;

public interface IProfileService
{
    public Task<ProfileResponse> GetProfileAsync(Guid jobHunterId);
    public Task<ProfileResponse> UpdateProfileAsync(ProfileRequest profileRequest);
}