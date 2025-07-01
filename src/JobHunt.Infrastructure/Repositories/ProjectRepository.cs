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

    public async Task<int> FinishedProjectsCountAsync(Guid userId)
    {
        return await _dbContext.Projects
            .Where(project =>
                project.ProjectOwner.Id == userId && project.EndDate <= DateTime.Now)
            .CountAsync();
    }

    public async Task<List<Project>> GetAllProjectsWithFilterAsync(Guid userId, string searchTerm, List<string> technologiesOrSkills)
    {
        List<Project> projects = await _dbContext.Projects
            .Include(p => p.ProjectOwner)
            .Where(p => p.ProjectOwner.Id == userId &&
                        p.ProjectTitle!.Contains(searchTerm))
            .ToListAsync();

        if (technologiesOrSkills.Count == 0) return projects;

        return projects.Where(
                    project =>
                        (project.TechnologiesOrSkills ?? []).ToHashSet()
                        .Intersect(technologiesOrSkills.ToHashSet()).Any()).ToList();
    }

    public async Task<Project?> GetByIdAsync(Guid projectId)
    {
        return await _dbContext.Projects
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.ProjectId == projectId);
    }

    public async Task<int> ProjectsCountAsync(Guid userId)
    {
        return await _dbContext.Projects.Where(p => p.ProjectOwner.Id == userId).CountAsync();
    }

    public async Task<int> RoleActedInProjectsCountAsync(Guid userId)
    {
        var projects = await _dbContext.Projects
            .Where(p => p.ProjectOwner.Id == userId)
            .ToListAsync(); // Bring data to client first

        var distinctRoles = projects
            .Where(p => p.Roles != null)
            .SelectMany(p => p.Roles!)
            .Distinct()
            .Count();

        return distinctRoles;
    }

    public async Task<int> TechnologyUsedInProjectsCountAsync(Guid userId)
    {
        var projects = await _dbContext.Projects
            .Where(p => p.ProjectOwner.Id == userId)
            .ToListAsync(); // Bring data to client first

        var distinctTechSkill = projects
            .Where(p => p.TechnologiesOrSkills != null)
            .SelectMany(p => p.TechnologiesOrSkills!)
            .Distinct()
            .Count();

        return distinctTechSkill;
    }

    public async Task<List<string>> TopFiveMostUsedTechnologyAsync(Guid userId)
    {
        // Top K frequent element - LeetCode :>>

        List<Project> res = await _dbContext.Projects
            .Where(p => p.ProjectOwner.Id == userId)
            .ToListAsync();

        List<string> techs = res
            .Where(p => p.TechnologiesOrSkills != null)
            .SelectMany(p => p.TechnologiesOrSkills!)
            .Distinct().ToList();

        Dictionary<string, int> frequentTech = [];
        foreach (var tech in techs)
        {
            if (frequentTech.ContainsKey(tech)) frequentTech[tech] += 1;
            else frequentTech[tech] = 1;
        }

        PriorityQueue<string, int> maxHeap = new();
        foreach (var tech in frequentTech)
        {
            maxHeap.Enqueue(tech.Key, -tech.Value);
        }

        List<string> finalResult = [];

        for (int i = 0; i < 5; ++i)
        {
            if (maxHeap.TryDequeue(out string? tech, out int _) && tech != null)
            {
                finalResult.Add(tech);
            }
            else
            {
                break;
            }
        }

        return finalResult;
    }

    public async Task<Project> UpdateAsync(Project project)
    {
        _dbContext.Projects.Update(project);
        await _dbContext.SaveChangesAsync();
        return project;
    }
}