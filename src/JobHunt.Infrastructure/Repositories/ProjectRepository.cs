using JobHunt.Core.Domain.Entities;
using JobHunt.Core.Domain.RepositoryContracts;
using JobHunt.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace JobHunt.Infrastructure.Repositories;

public class ProjectRepository(ApplicationDbContext dbContext) : IProjectRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<Project> AddProjectAsync(Project project)
    {
        _dbContext.Projects.Add(project);
        await _dbContext.SaveChangesAsync();
        return project; 
    }

    public async Task<Project?> DeleteAsync(Guid projectId)
    {
        Project? proj = _dbContext.Projects.Find(projectId);
        if (proj is null)
        {
            return null;
        }
        _dbContext.Projects.Remove(proj);
        await _dbContext.SaveChangesAsync();
        return proj;
    }


    public async Task<List<Project>> GetAllProjectsWithFilterAsync(Guid userId, string searchTerm, List<string> technologiesOrSkills)
    {
        return await _dbContext.Projects
            .Include(p => p.ProjectOwner)
            .Where(p => p.ProjectOwner.JobHunterId == userId &&
                        p.ProjectTitle!.Contains(searchTerm) &&
                        (p.TechnologiesOrSkills ?? new List<string>()).Any(t => technologiesOrSkills.Contains(t)))
            .ToListAsync();
    }

    public async Task<Project?> GetByIdAsync(Guid projectId)
    {
        return await _dbContext.Projects
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.ProjectId == projectId);
    }

    public async Task<Project> UpdateAsync(Project project)
    {
        _dbContext.Projects.Update(project);
        await _dbContext.SaveChangesAsync();
        return project;
    }
}