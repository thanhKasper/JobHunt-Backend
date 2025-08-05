using JobHunt.Core.Domain.Entities;

namespace JobHunt.Core.Domain.RepositoryContracts;

public interface IJobFilterReadRepository
{
    Task<List<JobFilter>> GetAllJobFiltersAsync();
    Task<List<JobFilter>> GetBatchJobFiltersUpToAmountAsync(int amount);
    bool IsGetAllJobFilters();
}