using JobHunt.Core.Domain.RepositoryContracts;
using JobHunt.Core.DTO;
using JobHunt.Core.ServiceContracts;

namespace JobHunt.Core.Services;

public class JobViewService(IJobViewRepository jobViewRepo) : IJobViewService
{
    private readonly IJobViewRepository _jobViewRepo = jobViewRepo;

    public async Task<List<JobResponse>> GetAllMatchingJobs()
    {
        return (await _jobViewRepo.GetAllJobs() ?? []).ToJobResponseList();
    }

    public async Task<List<JobResponse>> GetAllMatchingJobsBaseOnJobFilter(Guid? jobfilterId)
    {
        if (jobfilterId == null)
        {
            return [];
        }
        if (jobfilterId == Guid.Empty)
        {
            return [];
        }
        return (await _jobViewRepo.GetAllJobsBaseOnJobFilterId(jobfilterId.Value) ?? []).ToJobResponseList();
    }
}