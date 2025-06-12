using JobHunt.Core.Domain.Entities;
using JobHunt.Core.Domain.RepositoryContracts;

namespace JobHunt.Infrastructure.Repositories;

public class JobFilterRepositories : IJobFilterRepository
{
    public Task<JobFilter?> FindOneJobFilterById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<JobFilter[]?> GetAllJobFilters()
    {
        throw new NotImplementedException();
    }

    public Task<JobFilter?> RemoveJobFilter(JobFilter jobFilter)
    {
        throw new NotImplementedException();
    }

    public Task<JobFilter?> AddJobFilter(JobFilter jobFilter)
    {
        throw new NotImplementedException();
    }
}