using JobHunt.Core.Domain.Entities;
using JobHunt.Core.Helpers;

namespace JobHunt.Core.DTO;

public class ProjectResponse
{
    public Guid ProjectId { get; set; }
    public string? ProjectTitle { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; }
    public List<string>? Roles { get; set; }
    public List<string>? TechnologiesOrSkills { get; set; }
    public List<string>? Features { get; set; }
    public string? ProjectLink { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        ProjectResponse other = (ProjectResponse)obj;
        return ProjectId == other.ProjectId &&
            ProjectTitle == other.ProjectTitle &&
            StartDate == other.StartDate &&
            EndDate == other.EndDate &&
            Description == other.Description &&
            Utils.CompareArrayOfString(Features, other.Features) &&
            Utils.CompareArrayOfString(TechnologiesOrSkills, other.TechnologiesOrSkills) &&
            Utils.CompareArrayOfString(Roles, other.Roles) &&
            ProjectLink == other.ProjectLink;
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

public static class ProjectResponseExtension
{
    public static ProjectResponse ToProjectResponse(this Project project)
    {
        return new ProjectResponse()
        {
            Description = project.Description,
            EndDate = project.EndDate,
            Features = project.Features,
            ProjectId = project.ProjectId,
            ProjectLink = project.ProjectLink,
            ProjectTitle = project.ProjectTitle,
            Roles = project.Roles,
            StartDate = project.StartDate,
            TechnologiesOrSkills = project.TechnologiesOrSkills,
        };
    }
}