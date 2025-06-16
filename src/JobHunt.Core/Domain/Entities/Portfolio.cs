using System.ComponentModel.DataAnnotations;

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
    [MaxLength(100)]
    [Required]
    public List<string>? Role { get; set; }
    public List<string>? TechnologiesOrSkills { get; set; }
    public List<string>? Features { get; set; }
    [MaxLength(500)]
    public string? ProjectLink { get; set; }

    // Navigation property
    public JobHunter User { get; set; } = null!;
}