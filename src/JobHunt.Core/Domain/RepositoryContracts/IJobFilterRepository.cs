using JobHunt.Core.Domain.Entities;

namespace JobHunt.Core.Domain.RepositoryContracts;

public interface IJobFilterRepository
{
    public Task<JobFilter[]> GetAllJobFilters();
    public Task<JobFilter> FindOneJobFilterById(Guid id);
}