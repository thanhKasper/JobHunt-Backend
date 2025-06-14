using JobHunt.Core.DTO;

namespace JobHunt.Core.ServiceContracts;

public interface IJobViewService
{
    Task<List<JobResponse>> GetAllMatchingJobs();
    Task<List<JobResponse>> GetAllMatchingJobsBaseOnJobFilter(Guid? jobfilterId);
}