using JobHunt.Core.Domain.Entities;
using JobHunt.Core.Domain.RepositoryContracts;
using JobHunt.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace JobHunt.Infrastructure.Repositories;

public class JobFilterRepositories(ApplicationDbContext dbContext) : IJobFilterRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    public async Task<JobFilter?> FindOneJobFilterById(Guid id)
    {
        return await _dbContext.JobFilters.FindAsync(id);
    }

    public async Task<List<JobFilter>?> GetAllJobFilters()
    {
        return await _dbContext.JobFilters.Select(jobFilter => jobFilter).ToListAsync();
    }

    public async Task<JobFilter?> RemoveJobFilterById(Guid id)
    {
        JobFilter? jobFilter = await _dbContext.JobFilters.FindAsync(id);
        if (jobFilter != null) _dbContext.JobFilters.Remove(jobFilter);
        await _dbContext.SaveChangesAsync();
        return jobFilter;
    }

    public async Task<JobFilter?> AddJobFilterAsync(JobFilter jobFilter, JobHunter user)
    {
        jobFilter.JobFilterOwner = user;
        await _dbContext.JobFilters.AddAsync(jobFilter);
        await _dbContext.SaveChangesAsync();
        return jobFilter;
    }
}