using JobHunt.Core.Domain.Entities;

namespace JobHunt.Core.DTO;

public class ProjectResponseSummary
{
    public Guid ProjectId { get; set; }
    public string? ProjectTitle { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public List<string>? Role { get; set; }
    public List<string>? TechnologiesOrSkills { get; set; }
    public string? ProjectLink { get; set; }


}

public static class ProjectResponseSummaryExtension
{
    public static ProjectResponseSummary ToProjectResponseSummary(this Project project)
    {
        return new ProjectResponseSummary()
        {
            ProjectId = project.ProjectId,
            ProjectTitle = project.ProjectTitle,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            Role = project.Role,
            TechnologiesOrSkills = project.TechnologiesOrSkills,
            ProjectLink = project.ProjectLink
        };
    }

    public static List<ProjectResponseSummary> ToProjectResponseSummaryList(this List<Project> projects)
    {
        return projects.Select(p => p.ToProjectResponseSummary()).ToList();
    }
}