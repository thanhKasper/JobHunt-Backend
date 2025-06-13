using JobHunt.Core.Domain.Entities;

namespace JobHunt.Core.Domain.RepositoryContracts;

public interface IJobFilterRepository
{
    public Task<List<JobFilter>?> GetAllJobFilters();
    public Task<JobFilter?> FindOneJobFilterById(Guid id);
    public Task<JobFilter?> AddJobFilter(JobFilter jobFilter);
    public Task<JobFilter?> RemoveJobFilterById(Guid id);
}