using JobHunt.Core.DTO;

namespace JobHunt.Core.ServiceContracts;

public interface IMajorService
{
    Task<List<MajorKeyValuePair>> GetMajorListAsync();
}