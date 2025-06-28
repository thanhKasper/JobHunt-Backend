using JobHunt.Core.DTO;

namespace JobHunt.Core.ServiceContracts;

/// <summary>
/// This interface handles all the job filters requirements focusing on a specific user.
/// </summary>
public interface IJobFilterByUserService
{
    public Task<JobFilterListResponse> GetAllJobFiltersFromUser(Guid? userId);
}