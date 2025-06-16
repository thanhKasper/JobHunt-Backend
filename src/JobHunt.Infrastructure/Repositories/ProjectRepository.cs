using JobHunt.Core.Domain.Entities;
using JobHunt.Core.Domain.RepositoryContracts;
using JobHunt.Infrastructure.DatabaseContext;

namespace JobHunt.Infrastructure.Repositories;

public class ProjectRepository(ApplicationDbContext dbContext) : IProjectRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public Task<Project> AddProjectAsync(Project project)
    {
        throw new NotImplementedException();
    }

    public Task<Project?> DeleteAsync(Guid projectId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsAsync(Guid projectId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Project>> GetAllProjectsWithFilterAsync(Guid userId, string? searchTerm = null, List<string>? technologiesOrSkills = null)
    {
        throw new NotImplementedException();
    }

    public Task<Project?> GetByIdAsync(Guid projectId)
    {
        throw new NotImplementedException();
    }

    public Task<Project> UpdateAsync(Project project)
    {
        throw new NotImplementedException();
    }
}