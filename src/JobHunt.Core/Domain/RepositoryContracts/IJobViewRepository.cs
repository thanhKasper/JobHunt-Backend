using JobHunt.Core.Domain.Entities;

namespace JobHunt.Core.Domain.RepositoryContracts;

public interface IJobViewRepository
{
    List<Job> GetAllJobs();
    List<Job> GetAllJobsBaseOnJobFilterId(Guid jobFilterId);
}