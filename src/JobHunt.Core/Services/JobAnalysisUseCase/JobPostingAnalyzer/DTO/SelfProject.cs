using JobHunt.Core.Domain.Entities;

namespace JobHunt.Core.Services.JobAnalysisUseCase.JobPostingAnalyzer.DTO;

public class SelfProject
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public List<string> TechStack { get; set; } = [];
    public List<string> Tools { get; set; } = [];
    public List<string> Features { get; set; } = [];
    public List<string> Roles { get; set; } = [];


    public static SelfProject ToSelfProject(Project project)
    {
        if (string.IsNullOrEmpty(project.ProjectTitle) && 
            string.IsNullOrEmpty(project.Description))
        {
            throw new ArgumentException("Project title and description cannot be empty");
        }
        return new SelfProject()
        {
            Title = project.ProjectTitle ?? "",
            Description = project.Description ?? "",
            Features = project.Features.Select(feat => feat.Feature!).ToList(),
            Roles = project.Roles.Select(role => role.ProjectOwnerRole!).ToList(),
            TechStack = project.Technologies.Select(tech => tech.TechnologyName!).ToList(),
            Tools = project.Tools.Select(tool => tool.ToolName!).ToList()
        };
    }

    public static List<SelfProject> ToSelfProjectList(List<Project> projects)
    {
        return projects.Select(ToSelfProject).ToList();
    }
}