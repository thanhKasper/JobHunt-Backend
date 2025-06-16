namespace JobHunt.Core.Domain.RepositoryContracts;

using System;
using System.Threading.Tasks;
using JobHunt.Core.Domain.Entities;

public interface IProfileRepository
{
    Task<JobHunter?> GetProfileAsync(Guid jobHunterId);
    Task<JobHunter> UpdateProfileAsync(JobHunter jobHunter);
}
