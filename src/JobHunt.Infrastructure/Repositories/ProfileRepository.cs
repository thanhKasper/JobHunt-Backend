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
        JobHunter user = await _dbContext.JobHunters.FindAsync(jobHunter.Id)
            ?? throw new ArgumentException($"Profile with ID {jobHunter.Id} not found.");
        user.AboutMe = jobHunter.AboutMe;
        user.FullName = jobHunter.FullName;
        user.WorkingEmail = jobHunter.WorkingEmail;
        user.DateOfBirth = jobHunter.DateOfBirth;
        user.Address = jobHunter.Address;
        user.Education = jobHunter.Education;
        user.University = jobHunter.University;
        user.Major = jobHunter.Major;
        user.PhoneNumber = jobHunter.PhoneNumber;
        user.Awards = jobHunter.Awards;
        _dbContext.JobHunters.Update(user);
        await _dbContext.SaveChangesAsync();
        return user;
    }
}