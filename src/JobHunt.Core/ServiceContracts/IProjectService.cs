using JobHunt.Core.DTO;

namespace JobHunt.Core.ServiceContracts;

public interface IProjectService
{
    // Create operations
    Task<ProjectResponse> CreateProjectAsync(Guid? userId, ProjectRequest? request);

    // Read operations
    Task<ProjectResponse> GetProjectByIdAsync(Guid? projectId);

    // Search and filter operations
    Task<List<ProjectResponseSummary>> FilterProjectsAsync(Guid? userId, string? searchTerm, List<string>? technologiesOrSkills);

    // Update operations
    Task<ProjectResponse> UpdateProjectAsync(Guid? projectId, ProjectRequest? request);

    // Delete operations
    Task<ProjectResponse> DeleteProjectAsync(Guid? projectId);
    Task<List<ProjectResponse>> DeleteMultipleProjectsAsync(IEnumerable<Guid?>? projectIds);
}