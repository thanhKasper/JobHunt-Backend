using JobHunt.Core.Domain.Entities;

namespace JobHunt.Core.Domain.RepositoryContracts;

public interface IProjectRepository
{
    // Create operations
    Task<Project> AddAsync(Project project);

    // Read operations
    Task<Project?> GetByIdAsync(Guid projectId);
    Task<List<Project>> GetAllProjectsByUserIdAsync(Guid userId);

    // Search and filter operations
    Task<List<Project>> SearchAsync(Guid userId, string searchTerm);
    Task<List<Project>> GetByTechOrSkillAsync(Guid userId, List<string> technologies);

    // Update operations
    Task<Project> UpdateAsync(Project project);

    // Delete operations
    Task<Project?> DeleteAsync(Guid projectId);
    Task<bool> DeleteRangeAsync(IEnumerable<Guid> projectIds);

    // Existence and validation
    Task<bool> ExistsAsync(Guid projectId);

}