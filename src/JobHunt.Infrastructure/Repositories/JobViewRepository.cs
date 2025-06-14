using JobHunt.Core.Domain.Entities;
using JobHunt.Core.Domain.RepositoryContracts;
using JobHunt.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace JobHunt.Infrastructure.Repositories;

public class JobViewRepository(ApplicationDbContext dbContext) : IJobViewRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<List<Job>?> GetAllJobs()
    {
        return await _dbContext.Jobs
            .Include(job => job.Company)
            .Include(job => job.JobFilter)
            .ToListAsync();
    }

    public async Task<List<Job>?> GetAllJobsBaseOnJobFilterId(Guid jobFilterId)
    {
        return await _dbContext.Jobs
            .Include(job => job.Company)
            .Include(job => job.JobFilter)
            .Where(job => job.JobFilter.JobFilterId == jobFilterId)
            .OrderByDescending(job => job.PublishedDate)
            .ToListAsync();
    }
}