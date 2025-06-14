using JobHunt.Core.Domain.Entities;

namespace JobHunt.Core.Domain.RepositoryContracts;

public interface IJobViewRepository
{
    Task<List<Job>> GetAllJobs();
    Task<List<Job>> GetAllJobsBaseOnJobFilterId(Guid jobFilterId);
}