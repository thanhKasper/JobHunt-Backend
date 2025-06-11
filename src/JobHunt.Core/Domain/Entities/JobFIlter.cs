using System.ComponentModel.DataAnnotations;

namespace JobHunt.Core.Domain.Entities;

public class JobFilter
{
    [Key]
    public Guid? JobFilterId { get; set; }
    [Required]
    public string? FilterTitle { get; set; }
    [Required]
    public string? Occupation { get; set; }
    public string? JobLevel { get; set; }
    public int? YearsOfExperience { get; set; }
    public List<string>? TechnicalKnowledge { get; set; }
    public List<string>? SoftSkills { get; set; }
    public List<string>? Tools { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? LastUpdated { get; set; }
}