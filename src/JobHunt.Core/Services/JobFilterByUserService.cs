using JobHunt.Core.Domain.Entities;
using JobHunt.Core.Domain.RepositoryContracts;
using JobHunt.Core.DTO;
using JobHunt.Core.ServiceContracts;
using Microsoft.AspNetCore.Identity;

namespace JobHunt.Core.Services;


public class JobFilterByUserService(
    IJobFilterRepository jobfilterRepo,
    UserManager<JobHunter> userManager) : IJobFilterByUserService
{
    private readonly IJobFilterRepository _jobfilterRepo = jobfilterRepo;
    private readonly UserManager<JobHunter> _userManager = userManager;

    /// <summary>
    /// Find all the job filters that are created by the specific user
    /// </summary>
    /// <param name="userId">The unique ID of a user</param>
    /// <returns>A list of job filters created by users, empty list if the user have not created any job filters yet.</returns>
    /// <exception cref="ArgumentNullException">Throws if the <c>userId</c> is null</exception>
    /// <exception cref="ArgumentException">Throws if there is no user with the ID <c>userId</c></exception>
    public async Task<JobFilterListResponse> GetAllJobFiltersFromUserAsync(Guid? userId)
    {
        // Empty argument
        if (userId is null)
        {
            throw new ArgumentNullException(nameof(userId));
        }
        else
        {
            var user = await _userManager.FindByIdAsync(userId.ToString()!)
                ?? throw new ArgumentException("User Not Found");

            int totalJobsCount = await _jobfilterRepo.GetTotalJobFiltersOfUserAsync(user.Id);
            int activeJobsCount = await _jobfilterRepo.GetTotalActiveJobFiltersOfUserAsync(user.Id);
            List<JobFilter> jobFilters = await _jobfilterRepo.GetAllJobFiltersOfUserAsync(user.Id);


            return new JobFilterListResponse()
            {
                JobFilters = jobFilters.ToJobFilterResponseSimpleList(),
                TotalActiveJobs = activeJobsCount,
                TotalJobs = totalJobsCount
            };
        }

    }
}