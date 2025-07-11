using JobHunt.Core.Domain.Entities;
using JobHunt.Core.Domain.RepositoryContracts;
using JobHunt.Core.Domain.ValueObjects;
using JobHunt.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace JobHunt.Infrastructure.Repositories;

public class JobFilterRepository(ApplicationDbContext dbContext) : IJobFilterRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    public async Task<JobFilter?> FindOneJobFilterByIdAsync(Guid id)
    {
        return await _dbContext.JobFilters
            .Where(jf => jf.JobFilterId == id)
            .Include(jf => jf.Level)
            .Include(jf => jf.Occupation)
            .Include(jf => jf.SpecializedKnowledges)
            .Include(jf => jf.Tools)
            .Include(jf => jf.SoftSkills)
            .Include(jf => jf.Languages)
            .AsSplitQuery() // Avoid Cartesian Explosion
            .AsNoTracking()
            .Select(
                jobfilter => new JobFilter()
                {
                    CreatedAt = jobfilter.CreatedAt,
                    FilterTitle = jobfilter.FilterTitle,
                    IsActive = jobfilter.IsActive,
                    IsStarred = jobfilter.IsStarred,
                    JobFilterId = jobfilter.JobFilterId,
                    JobFilterOwner = jobfilter.JobFilterOwner,
                    Languages = jobfilter.Languages,
                    LastUpdated = jobfilter.LastUpdated,
                    Level = jobfilter.Level,
                    Location = jobfilter.Location,
                    MatchJobList = jobfilter.MatchJobList,
                    Occupation = jobfilter.Occupation,
                    SoftSkills = jobfilter.SoftSkills,
                    SpecializedKnowledges = jobfilter.SpecializedKnowledges,
                    Tools = jobfilter.Tools,
                    YearsOfExperience = jobfilter.YearsOfExperience,
                    JobsCount = (jobfilter.MatchJobList != null)
                        ? jobfilter.MatchJobList.Count()
                        : 0
                })
            .FirstAsync();
    }

    public async Task<List<JobFilter>?> GetAllJobFiltersAsync()
    {
        return await _dbContext.JobFilters.ToListAsync();
    }

    public async Task<JobFilter?> RemoveJobFilterByIdAsync(Guid id)
    {
        JobFilter? jobFilter = await _dbContext
            .JobFilters
            .Include(jf => jf.Occupation)
            .Where(jf => jf.JobFilterId == id).FirstAsync();
        if (jobFilter != null) _dbContext.JobFilters.Remove(jobFilter);
        await _dbContext.SaveChangesAsync();
        return jobFilter;
    }

    public async Task<JobFilter?> AddJobFilterAsync(JobFilter jobFilter, JobHunter user)
    {
        JobLevel? joblevel = await _dbContext.JobLevels.FindAsync(jobFilter.Level.JobLevelId!);
        var jobField = await _dbContext.JobFields.FindAsync(jobFilter.Occupation.JobFieldId);
        jobFilter.JobFilterOwner = user;
        jobFilter.Level = joblevel!;
        jobFilter.Occupation = jobField!;
        await _dbContext.JobFilters.AddAsync(jobFilter);
        await _dbContext.SaveChangesAsync();
        return jobFilter;
    }

    public async Task<int> GetTotalJobFiltersOfUserAsync(Guid id)
    {
        return await _dbContext.JobFilters
            .AsNoTracking()
            .Include(jf => jf.JobFilterOwner)
            .Where(jf => jf.JobFilterOwner.Id == id)
            .Include(jf => jf.Level)
            .Include(jf => jf.Occupation)
            .AsSplitQuery()
            .CountAsync();
    }

    public async Task<int> GetTotalActiveJobFiltersOfUserAsync(Guid id)
    {
        return await _dbContext.JobFilters
            .Include(jf => jf.JobFilterOwner)
            .AsNoTracking() // Improve performance with AsNoTracking (Work only in Read methods)
            .Where(jf => jf.JobFilterOwner.Id == id && jf.IsActive == true)
            .CountAsync();
    }

    public async Task<List<JobFilter>> GetAllJobFiltersOfUserAsync(Guid id)
    {
        return await _dbContext.JobFilters
            .Include(jf => jf.JobFilterOwner)
            .AsNoTracking()
            .Where(jf => jf.JobFilterOwner.Id == id)
            .OrderByDescending(jf => jf.IsStarred)
            .ThenByDescending(jf => jf.IsActive)
            .Select(jobfilter => new JobFilter
            {
                CreatedAt = jobfilter.CreatedAt,
                FilterTitle = jobfilter.FilterTitle,
                IsActive = jobfilter.IsActive,
                IsStarred = jobfilter.IsStarred,
                JobFilterId = jobfilter.JobFilterId,
                JobFilterOwner = jobfilter.JobFilterOwner,
                Languages = jobfilter.Languages,
                LastUpdated = jobfilter.LastUpdated,
                Level = jobfilter.Level,
                Location = jobfilter.Location,
                MatchJobList = jobfilter.MatchJobList,
                Occupation = jobfilter.Occupation,
                SoftSkills = jobfilter.SoftSkills,
                SpecializedKnowledges = jobfilter.SpecializedKnowledges,
                Tools = jobfilter.Tools,
                YearsOfExperience = jobfilter.YearsOfExperience,
                JobsCount = (jobfilter.MatchJobList != null) ? jobfilter.MatchJobList.Count() : 0
            })
            .ToListAsync();
    }

    public async Task<bool> ToggleJobFilterActiveStateAsync(JobFilter jobfilter)
    {
        bool currentActiveState = jobfilter.IsActive!.Value;
        jobfilter.IsActive = !currentActiveState;
        await _dbContext.SaveChangesAsync();
        return !currentActiveState;
    }

    public async Task<bool> ToggleJobFilterStarStateAsync(JobFilter jobfilter)
    {
        bool currentStarState = jobfilter.IsStarred!.Value;
        jobfilter.IsStarred = !currentStarState;
        await _dbContext.SaveChangesAsync();
        return !currentStarState;
    }

    public async Task<List<JobField>> GetAllJobFieldsAsync()
    {
        return await _dbContext.JobFields.ToListAsync();
    }

    public async Task<List<JobLevel>> GetAllJobLevelsAsync()
    {
        return await _dbContext.JobLevels.ToListAsync();
    }
}