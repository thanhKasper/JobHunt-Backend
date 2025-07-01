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

    /// <summary>
    /// Get the general information about the list of projects uploaded by the user.
    /// </summary>
    /// <param name="userId">user id that project general info belongs to</param>
    /// <returns>The general information gathered from the list of project</returns>
    /// <exception cref="ArgumentNullException">Throw null userId is null</exception>
    /// <exception cref="ArgumentException">Throw if cannot found user with that userId</exception>
    Task<ProjectGeneralInfoResponse> GetGeneralProjectInfoFromUserAsync(Guid? userId);
}