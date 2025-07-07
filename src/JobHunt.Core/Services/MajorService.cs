namespace JobHunt.Core.Services;

using JobHunt.Core.Domain.RepositoryContracts;
using JobHunt.Core.DTO;
using JobHunt.Core.ServiceContracts;

public class MajorService(
    IProfileRepository profileRepository
) : IMajorService
{
    private readonly IProfileRepository _profileRepository = profileRepository;
    public async Task<List<MajorKeyValuePair>> GetMajorListAsync()
    {
        var majorList = await _profileRepository.GetMajorListAsync();
        return majorList.Select(major => new MajorKeyValuePair
        {
            Key = major.MajorId.ToString(),
            Value = major.VietNameseName
        }).ToList();
    }
}
