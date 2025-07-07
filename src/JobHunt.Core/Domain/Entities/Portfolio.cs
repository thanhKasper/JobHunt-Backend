using System.ComponentModel.DataAnnotations;
using JobHunt.Core.Domain.ValueObjects;

namespace JobHunt.Core.Domain.Entities;

public class Project
{
    [Key]
    public Guid ProjectId { get; set; }
    [MaxLength(200)]
    [Required]
    public string? ProjectTitle { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    [MaxLength(2000)]
    [Required]
    public string? Description { get; set; }
    [MaxLength(500)]
    public string? ProjectLink { get; set; }
    [MaxLength(500)]
    public string? DemoLink { get; set; }

    #region Navigational Property
    // Navigation property
    public JobHunter ProjectOwner { get; set; } = null!;
    public List<TechnologyOrSkill> TechnologiesOrSkills { get; set; } = [];
    public List<Role> Roles { get; set; } = [];
    public List<ProjectFeature> Features { get; set; } = [];
    #endregion
}