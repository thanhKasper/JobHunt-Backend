using JobHunt.Core.Domain.RepositoryContracts;
using JobHunt.Core.DTO;
using JobHunt.Core.ServiceContracts;

namespace JobHunt.Core.Services;

public class JobViewService(IJobViewRepository jobViewRepo) : IJobViewService
{
    private readonly IJobViewRepository _jobViewRepo = jobViewRepo;

    public Task<List<JobResponse>> GetAllMatchingJobs()
    {
        throw new NotImplementedException();
    }

    public Task<List<JobResponse>> GetAllMatchingJobsBaseOnJobFilter(Guid? jobfilterId)
    {
        throw new NotImplementedException();
    }
}