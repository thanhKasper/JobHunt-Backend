using JobHunt.Core.Domain.Entities;

namespace JobHunt.Core.ServiceContracts.JobAnalysisServiceContract;

public interface IJobExpectationRepository
{
    Task<List<JobFilter>> GetAllJobFiltersAsync();
    Task<List<JobFilter>> GetBatchJobFiltersUpToAmountAsync(int amount);
    bool IsGetAllJobFilters();
}