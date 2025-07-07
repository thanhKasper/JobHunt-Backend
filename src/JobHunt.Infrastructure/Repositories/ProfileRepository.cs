using System.Security;
using JobHunt.Core.Domain.Entities;
using JobHunt.Core.Domain.RepositoryContracts;
using JobHunt.Core.Domain.ValueObjects;
using JobHunt.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace JobHunt.Infrastructure.Repositories;

public class ProfileRepository(ApplicationDbContext dbContext) : IProfileRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<JobHunter?> GetProfileAsync(Guid jobHunterId)
    {
        return await _dbContext.JobHunters
            .AsNoTracking()
            .Include(jh => jh.Major)
            .Include(jh => jh.Education)
            .Include(jh => jh.Awards)
            .AsSplitQuery()
            .Where(jh => jh.Id == jobHunterId)
            .FirstAsync();
    }

    public async Task<JobHunter> UpdateProfileAsync(JobHunter jobHunter)
    {
        JobHunter user = await _dbContext.JobHunters
            .Include(jh => jh.Major)
            .Include(jh => jh.Education)
            .Include(jh => jh.Awards)
            .AsSplitQuery()
            .Where(jh => jh.Id == jobHunter.Id)
            .FirstAsync()
            ?? throw new ArgumentException($"Profile with ID {jobHunter.Id} not found.");
        Major? chosenMajor = await _dbContext.Majors.FindAsync(jobHunter.Major.MajorId)
            ?? throw new ArgumentException($"Major with ID {jobHunter.Major.MajorId} not found.");
        Education? chosenEducation = await _dbContext.Educations
            .FindAsync(jobHunter.Education.EducationId)
            ?? throw new ArgumentException(
                $"Education with ID {jobHunter.Education.EducationId} not found.");


        user.AboutMe = jobHunter.AboutMe;
        user.FullName = jobHunter.FullName;
        user.WorkingEmail = jobHunter.WorkingEmail;
        user.DateOfBirth = jobHunter.DateOfBirth;
        user.Address = jobHunter.Address;
        user.Education = chosenEducation;
        user.University = jobHunter.University;
        user.Major = chosenMajor;
        user.PhoneNumber = jobHunter.PhoneNumber;


        _dbContext.RemoveRange(user.Awards);
        user.Awards.AddRange(jobHunter.Awards);

        _dbContext.JobHunters.Update(user);
        await _dbContext.SaveChangesAsync();
        return user;
    }

    public async Task<List<Major>> GetMajorListAsync()
    {
        return await _dbContext.Majors.ToListAsync();
    }

    public async Task<List<Education>> GetDegreeListAsync()
    {
        return await _dbContext.Educations.ToListAsync();
    }
}