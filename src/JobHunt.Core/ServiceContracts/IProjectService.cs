using JobHunt.Core.DTO;

namespace JobHunt.Core.ServiceContracts;

public interface IProjectService
{
    // Create operations
    Task<ProjectResponse> CreateProjectAsync(Guid? userId, ProjectRequest? request);

    // Read operations
    Task<ProjectResponse> GetProjectByIdAsync(Guid? projectId);
    Task<List<ProjectResponseSummary>> GetAllProjectsFromUserAsync(Guid? userId);

    // Search and filter operations
    Task<List<ProjectResponseSummary>> SearchProjectsFromUserAsync(Guid? userId, string searchTerm);
    Task<List<ProjectResponseSummary>> GetProjectsByTechOrSkillAsync(Guid? userId, List<string> technologies);

    // Update operations
    Task<ProjectResponse> UpdateProjectAsync(Guid? projectId, ProjectRequest? request);

    // Delete operations
    Task<ProjectResponse> DeleteProjectAsync(Guid projectId);
    Task<bool> DeleteMultipleProjectsAsync(IEnumerable<Guid> projectIds);
}