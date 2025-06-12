using JobHunt.Core.Domain.Entities;

namespace JobHunt.Core.Domain.RepositoryContracts;

public interface IJobFilterRepository
{
    public Task<JobFilter[]?> GetAllJobFilters();
    public Task<JobFilter?> FindOneJobFilterById(Guid id);
    public Task<JobFilter?> AddJobFilter(JobFilter jobFilter);
    public Task<JobFilter?> RemoveJobFilter(JobFilter jobFilter);
}