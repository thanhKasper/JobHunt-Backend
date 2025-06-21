using System.ComponentModel.DataAnnotations;
using JobHunt.Core.Domain.ValueObjects;

namespace JobHunt.Core.Domain.Entities;

public class JobFilter
{
    [Key]
    public Guid JobFilterId { get; set; }
    [Required]
    [MaxLength(128)]
    public string? FilterTitle { get; set; }
    [Required]
    public JobField? Occupation { get; set; }
    public string? Location { get; set; }
    public JobLevel? Level { get; set; }
    public int? YearsOfExperience { get; set; }
    public List<string>? TechnicalKnowledge { get; set; }
    public List<string>? SoftSkills { get; set; }
    public List<string>? Tools { get; set; }
    public List<string>? Languages { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? LastUpdated { get; set; }
    public List<Job>? MatchJobList { get; set; }

    public void FillYearExp()
    {
        YearsOfExperience = Level switch
        {
            JobLevel.Intern => 0,
            JobLevel.Fresher => 0,
            JobLevel.Junior => 3,
            JobLevel.Middle => 5,
            _ => 20
        };
    }

    public void FillJobLevel()
    {
        Level = YearsOfExperience switch
        {
            < 1 => JobLevel.Fresher,
            <= 3 => JobLevel.Junior,
            <= 5 => JobLevel.Middle,
            _ => JobLevel.Senior,
        };
    }
}