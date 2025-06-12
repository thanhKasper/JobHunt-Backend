using JobHunt.Core.Domain.RepositoryContracts;
using JobHunt.Core.DTO;
using JobHunt.Core.ServiceContracts;

namespace JobHunt.Core.Services;

public class JobFilterService(IJobFilterRepository jobFilterRepo) : IJobFilterService
{
    private readonly IJobFilterRepository _jobFilterRepo = jobFilterRepo;
    public Task<JobFilterResponseDetail?> CreateNewJobFilter(JobFilterCreationRequest? jobFilterRequest)
    {
        throw new NotImplementedException();
    }

    public Task<JobFilterResponseSimple?> DeleteJobFilter(Guid? jobFilterId)
    {
        throw new NotImplementedException();
    }

    public Task<List<JobFilterResponseSimple>> GetAllJobFilterSimple()
    {
        throw new NotImplementedException();
    }

    public Task<JobFilterResponseDetail?> GetJobFilterDetail(Guid? jobFilterId)
    {
        throw new NotImplementedException();
    }
}