using System.ComponentModel.DataAnnotations;
using JobHunt.Core.Domain.ValueObjects;

namespace JobHunt.Core.Domain.Entities;

public class JobFilter
{
    [Key]
    public Guid JobFilterId { get; set; }
    [Required]
    public string? FilterTitle { get; set; }
    [Required]
    public JobField? Occupation { get; set; }
    public JobLevel? Level { get; set; }
    public int? YearsOfExperience { get; set; }
    public List<string>? TechnicalKnowledge { get; set; }
    public List<string>? SoftSkills { get; set; }
    public List<string>? Tools { get; set; }
    public List<string>? Languages { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? LastUpdated { get; set; }
    // public List<Job>? MatchJobList { get; set; }
}