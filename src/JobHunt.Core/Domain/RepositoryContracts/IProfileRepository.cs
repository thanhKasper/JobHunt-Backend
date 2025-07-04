namespace JobHunt.Core.Domain.RepositoryContracts;

using System;
using System.Threading.Tasks;
using JobHunt.Core.Domain.Entities;
using JobHunt.Core.Domain.ValueObjects;

public interface IProfileRepository
{
    Task<JobHunter?> GetProfileAsync(Guid jobHunterId);
    Task<JobHunter> UpdateProfileAsync(JobHunter jobHunter);
    Task<List<Major>> GetMajorListAsync();
    Task<List<Education>> GetDegreeListAsync();
}
