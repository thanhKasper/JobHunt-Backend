using System.ComponentModel.DataAnnotations;

namespace JobHunt.Core.Domain.Entities;

public class Project
{
    [Key]
    public Guid ProjectId { get; set; }
    public string? ProjectTitle { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; }
    public string? Role { get; set; }
    public List<string>? TechnologiesOrSkills { get; set; }
    public List<string>? Features { get; set; }
    public string? Link { get; set; }

    // Navigation property
    public JobHunter User { get; set; } = null!;
}