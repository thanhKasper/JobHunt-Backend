namespace JobHunt.Core.DTO;

public class ProjectResponse
{
    public Guid ProjectId { get; set; }
    public string ProjectTitle { get; set; } = string.Empty;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; }
    public List<string>? Role { get; set; }
    public List<string>? TechnologiesOrSkills { get; set; }
    public List<string>? Features { get; set; }
    public string? ProjectLink { get; set; }
}