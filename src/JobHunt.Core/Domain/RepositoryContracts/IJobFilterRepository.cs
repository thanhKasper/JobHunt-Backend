using JobHunt.Core.Domain.Entities;
using JobHunt.Core.Domain.ValueObjects;

namespace JobHunt.Core.Domain.RepositoryContracts;

public interface IJobFilterRepository
{
    public Task<List<JobFilter>?> GetAllJobFiltersAsync();
    public Task<JobFilter?> FindOneJobFilterByIdAsync(Guid id);
    public Task<JobFilter?> AddJobFilterAsync(JobFilter jobFilter, JobHunter user);
    public Task<JobFilter?> RemoveJobFilterByIdAsync(Guid id);
    public Task<int> GetTotalJobFiltersOfUserAsync(Guid id);
    public Task<int> GetTotalActiveJobFiltersOfUserAsync(Guid id);
    public Task<List<JobFilter>> GetAllJobFiltersOfUserAsync(Guid id);
    public Task<bool> ToggleJobFilterActiveStateAsync(JobFilter jobfilter);
    public Task<bool> ToggleJobFilterStarStateAsync(JobFilter jobfilter);
    public Task<List<JobField>> GetAllJobFieldsAsync();
    public Task<List<JobLevel>> GetAllJobLevelsAsync();
}