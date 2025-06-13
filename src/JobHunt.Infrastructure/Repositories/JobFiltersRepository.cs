using JobHunt.Core.Domain.Entities;
using JobHunt.Core.Domain.RepositoryContracts;

namespace JobHunt.Infrastructure.Repositories;

public class JobFilterRepositories : IJobFilterRepository
{
    public Task<JobFilter?> FindOneJobFilterById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<List<JobFilter>?> GetAllJobFilters()
    {
        throw new NotImplementedException();
    }

    public Task<JobFilter?> RemoveJobFilterById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<JobFilter?> AddJobFilter(JobFilter jobFilter)
    {
        throw new NotImplementedException();
    }
}