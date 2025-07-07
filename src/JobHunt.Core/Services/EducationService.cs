using JobHunt.Core.Domain.RepositoryContracts;
using JobHunt.Core.DTO;
using JobHunt.Core.ServiceContracts;

namespace JobHunt.Core.Services;

public class EducationService(
    IProfileRepository profileRepository
) : IEducationService
{
    private readonly IProfileRepository _profileRepository = profileRepository;
    /// <summary>
    /// Gets the list of degrees.
    /// </summary>
    /// <returns>A list of degrees as key-value pairs.</returns>
    public async Task<List<EducationKeyValuePair>> GetDegreeListAsync()
    {
        var degreeList = await _profileRepository.GetDegreeListAsync();
        return degreeList.Select(degree => new EducationKeyValuePair
        {
            Key = degree.EducationId.ToString(),
            Value = degree.VietNameseName
        }).ToList();
    }
}