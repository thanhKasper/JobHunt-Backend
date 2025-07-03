using System.ComponentModel.DataAnnotations;
using JobHunt.Core.Domain.Entities;
using JobHunt.Core.Domain.RepositoryContracts;
using JobHunt.Core.DTO;
using JobHunt.Core.ServiceContracts;
using Microsoft.AspNetCore.Identity;

namespace JobHunt.Core.Services;

public class ProjectService(
    IProjectRepository projectRepository,
    IProfileRepository profileRepository,
    UserManager<JobHunter> userManager) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IProfileRepository _profileRepository = profileRepository;
    private readonly UserManager<JobHunter> _userManager = userManager;

    public async Task<ProjectResponse> CreateProjectAsync(Guid? userId, ProjectRequest? request)
    {
        if (!userId.HasValue)
        {
            throw new ArgumentNullException(nameof(userId));
        }

        ArgumentNullException.ThrowIfNull(request, nameof(request));

        ValidationContext ctx = new(request);
        List<ValidationResult> validationResults = [];
        if (!Validator.TryValidateObject(request, ctx, validationResults, true))
        {
            throw new ArgumentException(validationResults.FirstOrDefault()?.ErrorMessage);
        }

        Project newProject = request.ToProject();

        // This was done before i learn about Microsoft Identity
        JobHunter? projectOwner = await _profileRepository.GetProfileAsync(userId.Value)
            ?? throw new ArgumentException($"User with ID {userId} not found.");

        newProject.ProjectOwner = projectOwner;

        var createdProject = await _projectRepository.AddProjectAsync(newProject);
        return createdProject.ToProjectResponse();
    }

    public async Task<List<ProjectResponse>> DeleteMultipleProjectsAsync(IEnumerable<Guid?>? projectIds)
    {
        ArgumentNullException.ThrowIfNull(projectIds, nameof(projectIds));
        IEnumerable<Task<ProjectResponse>> tasks = projectIds
            .Where(id => id.HasValue)
            .Select(async id =>
            {
                try
                {
                    return await DeleteProjectAsync(id);
                }
                catch
                {
                    return new ProjectResponse { ProjectId = Guid.Empty };
                }
            });

        ProjectResponse[] results = await Task.WhenAll(tasks);
        return results.Where(r => r.ProjectId != Guid.Empty).Select(r => r).ToList();
    }

    public async Task<ProjectResponse> DeleteProjectAsync(Guid? projectId)
    {
        if (!projectId.HasValue)
        {
            throw new ArgumentNullException(nameof(projectId));
        }

        Project? res = await _projectRepository.DeleteAsync(projectId.Value)
            ?? throw new ArgumentException($"Project with ID {projectId} not found.");

        return res.ToProjectResponse();
    }

    public async Task<List<ProjectResponse>> FilterProjectsAsync(Guid? userId, string? searchTerm, List<string>? technologiesOrSkills)
    {
        if (!userId.HasValue)
        {
            throw new ArgumentNullException(nameof(userId));
        }

        var matchProjects = await _projectRepository
            .GetAllProjectsWithFilterAsync(
                userId.Value,
                searchTerm ?? "",
                technologiesOrSkills ?? new List<string>());

        return matchProjects.ToProjectResponseList();
    }

    public async Task<ProjectGeneralInfoResponse> GetGeneralProjectInfoFromUserAsync(Guid? userId)
    {
        if (!userId.HasValue) throw new ArgumentNullException(nameof(userId), "userId cannot be empty");

        var user = await _userManager.FindByIdAsync(userId.Value.ToString())
            ?? throw new ArgumentException("User cannot be found");

        int projectCount = await _projectRepository.ProjectsCountAsync(user.Id);
        int completedProjectCount = await _projectRepository.FinishedProjectsCountAsync(user.Id);
        int techUsedCount = await _projectRepository.TechnologyUsedInProjectsCountAsync(user.Id);
        int roleJoinedCount = await _projectRepository.RoleActedInProjectsCountAsync(user.Id);
        List<string> topUsedTech = await _projectRepository.TopFiveMostUsedTechnologyAsync(user.Id);

        return new ProjectGeneralInfoResponse()
        {
            MostUsedTech = topUsedTech,
            TotalCompleteProjects = completedProjectCount,
            TotalProjects = projectCount,
            TotalRoles = roleJoinedCount,
            TotalUsedTools = techUsedCount,
        };
    }

    public async Task<ProjectResponse> GetProjectByIdAsync(Guid? projectId)
    {
        if (!projectId.HasValue)
        {
            throw new ArgumentNullException(nameof(projectId));
        }

        Project? project = await _projectRepository.GetByIdAsync(projectId.Value)
            ?? throw new ArgumentException($"Project with ID {projectId} not found.");

        return project.ToProjectResponse();
    }

    public async Task<ProjectResponse> UpdateProjectAsync(Guid? projectId, ProjectRequest? request)
    {
        if (!projectId.HasValue)
        {
            throw new ArgumentNullException(nameof(projectId));
        }

        ArgumentNullException.ThrowIfNull(request, nameof(request));

        ValidationContext ctx = new(request);
        List<ValidationResult> validationResults = [];
        if (!Validator.TryValidateObject(request, ctx, validationResults, true))
        {
            throw new ArgumentException(validationResults.FirstOrDefault()?.ErrorMessage);
        }

        Project? existingProject = await _projectRepository.GetByIdAsync(projectId.Value)
            ?? throw new ArgumentException($"Project with ID {projectId} not found.");

        Project updatedProject = await _projectRepository
            .UpdateAsync(existingProject, request.ToProject());
        return updatedProject.ToProjectResponse();
    }
}

