using JobHunt.Core.Domain.Entities;
using JobHunt.Core.Domain.RepositoryContracts;
using JobHunt.Infrastructure.DatabaseContext;

namespace JobHunt.Infrastructure.Repositories;

public class ProfileRepository(ApplicationDbContext dbContext) : IProfileRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<JobHunter?> GetProfileAsync(Guid jobHunterId)
    {
        return await _dbContext.JobHunters.FindAsync(jobHunterId);
    }

    public async Task<JobHunter> UpdateProfileAsync(JobHunter jobHunter)
    {
        _dbContext.JobHunters.Update(jobHunter);
        await _dbContext.SaveChangesAsync();
        return jobHunter;
    }
}