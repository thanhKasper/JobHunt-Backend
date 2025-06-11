using JobHunt.Core.DTO;

namespace JobHunt.Core.ServiceContracts;

public interface IJobFilterService
{
    public Task<JobFilterResponseSimple> CreateNewJobFilter(JobFilterCreationRequest jobFilterRequest);
    public Task<List<JobFilterResponseSimple>> GetAllJobFilterSimple();
    public Task<List<JobFilterResponseDetail>> GetAllJobFilterDetail();
    public Task<JobFilterResponseSimple> DeleteJobFilter(Guid jobFilterId);
}