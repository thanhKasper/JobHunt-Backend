using JobHunt.Core.Domain.Entities;
using JobHunt.Core.Domain.RepositoryContracts;
using JobHunt.Core.Domain.ValueObjects;
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

    public async Task<List<Project>> GetAllProjectsWithFilterAsync(Guid userId, string searchTerm, List<string> technologies)
    {
        List<Project> projects = await _dbContext.Projects
            .AsNoTracking()
            .Include(p => p.ProjectOwner)
            .Include(p => p.Technologies)
            .Include(p => p.Tools)
            .Include(p => p.Roles)
            .AsSplitQuery()
            .Where(p => p.ProjectOwner.Id == userId &&
                        p.ProjectTitle!.Contains(searchTerm))
            .ToListAsync();

        if (technologies.Count == 0) return projects;

        return projects.Where(
                    project =>
                        (project.Technologies.Select(tech => tech.TechnologyName) ?? [])
                        .ToHashSet()
                        .Intersect(technologies.ToHashSet()).Any()).ToList();
    }

    public async Task<Project?> GetByIdAsync(Guid projectId)
    {
        return await _dbContext.Projects
            .AsNoTracking()
            .Include(p => p.Technologies)
            .Include(p => p.Features)
            .Include(p => p.Roles)
            .AsSplitQuery()
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
            .Include(p => p.Roles)
            .ToListAsync(); // Bring data to client first

        var distinctRoles = projects
            .Where(p => p.Roles != null)
            .SelectMany(p => p.Roles!.Select(role => role.ProjectOwnerRole!.ToUpper()))
            .Distinct()
            .Count();

        return distinctRoles;
    }

    public async Task<int> TechnologyUsedInProjectsCountAsync(Guid userId)
    {

        List<Guid> jobSeekerProjectsList = await _dbContext.Projects
            .AsNoTracking()
            .Where(p => p.ProjectOwner.Id == userId)
            .Select(p => p.ProjectId)
            .ToListAsync();

        int technologiesCount = await _dbContext.Technologies
            .Where(tech => 
                tech.Project != null && 
                tech.TechnologyName != null &&
                jobSeekerProjectsList.Contains(tech.Project.ProjectId))
            .DistinctBy(tech => tech.TechnologyName!.ToUpper())
            .CountAsync();
        //var projects = await _dbContext.Projects
        //    .Where(p => p.ProjectOwner.Id == userId)
        //    .Include(p => p.Technologies)
        //    .ToListAsync(); // Bring data to client first

        //var distinctTechSkill = projects
        //    .Where(p => p.Technologies != null)
        //    .SelectMany(p => p.Technologies!
        //        .Select(tech => tech.TechnologyName!.ToUpper()));
        


        return technologiesCount;
    }

    public async Task<List<string>> TopFiveMostUsedTechnologyAsync(Guid userId)
    {
        // Top K frequent element - LeetCode :>>

        List<Project> res = await _dbContext.Projects
            .Where(p => p.ProjectOwner.Id == userId)
            .Include(p => p.Technologies)
            .Include(p => p.Roles)
            .AsSplitQuery()
            .AsNoTracking()
            .ToListAsync();

        List<string> techs = res
            .Where(p => p.Technologies != null)
            .SelectMany(p => p.Technologies.Select(tech => tech.TechnologyName!))
            .ToList();

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

    public async Task<Project> UpdateAsync(Project oldProject, Project newProject)
    {
        oldProject.ProjectLink = newProject.ProjectLink;
        oldProject.ProjectTitle = newProject.ProjectTitle;
        oldProject.StartDate = newProject.StartDate;
        oldProject.EndDate = newProject.EndDate;
        oldProject.Description = newProject.Description;
        oldProject.DemoLink = newProject.DemoLink;
        // Clear the old one and add a new list of result

        _dbContext.RemoveRange(
            oldProject.Technologies
            .Where(tech => tech.Project != null && tech.Project.ProjectId == oldProject.ProjectId));

        oldProject.Technologies.AddRange(newProject.Technologies);

        _dbContext.RemoveRange(
            oldProject.Features
            .Where(feature => feature.Project.ProjectId == oldProject.ProjectId));
        oldProject.Features.AddRange(newProject.Features);

        _dbContext.RemoveRange(
            oldProject.Roles
            .Where(role => role.Project.ProjectId == oldProject.ProjectId));
        oldProject.Roles.AddRange(newProject.Roles);

        _dbContext.Projects.Update(oldProject);
        await _dbContext.SaveChangesAsync();
        return oldProject;
    }
}