using JobHunt.Core.Domain.Entities;

namespace JobHunt.Core.Domain.RepositoryContracts;

public interface IProjectRepository
{
    // Create operations
    Task<Project> AddProjectAsync(Project project);

    // Read operations
    Task<Project?> GetByIdAsync(Guid projectId);
    Task<List<Project>> GetAllProjectsWithFilterAsync(Guid userId, string searchTerm, List<string> technologiesOrSkills);

    // Update operations
    Task<Project> UpdateAsync(Project project);

    // Delete operations
    Task<Project?> DeleteAsync(Guid projectId);

    // Existence and validation
    Task<bool> ExistsAsync(Guid projectId);

}