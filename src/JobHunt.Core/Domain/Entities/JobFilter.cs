using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JobHunt.Core.Domain.ValueObjects;

namespace JobHunt.Core.Domain.Entities;

public class JobFilter
{
    [Key]
    public Guid JobFilterId { get; set; }
    [DefaultValue(true)]
    public bool? IsActive { get; set; }
    [DefaultValue(false)]
    public bool? IsStarred { get; set; }
    [Required]
    [MaxLength(128)]
    public string? FilterTitle { get; set; }
    public string? Location { get; set; }
    public int? YearsOfExperience { get; set; }
    public List<string>? TechnicalKnowledge { get; set; }
    public List<string>? SoftSkills { get; set; }
    public List<string>? Tools { get; set; }
    public List<string>? Languages { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? LastUpdated { get; set; }

    [NotMapped]
    public int JobsCount { get; set; } = 0;

    [DefaultValue(0)]
    public int AverageCompatibility { get; set; } = 0;



    // Navigational Property
    [Required]
    public JobField Occupation { get; set; } = null!; // Required one-to-many relationship
    public JobLevel? Level { get; set; } // Optional one-to-many relationship
    public List<Job>? MatchJobList { get; set; }
    public JobHunter JobFilterOwner { get; set; } = null!;

    public void FillYearExp()
    {
        YearsOfExperience = Level!.JobLevelId switch
        {
            JobLevelKey.Intern => 0,
            JobLevelKey.Fresher => 1,
            JobLevelKey.Junior => 3,
            _ => 5
        };
    }

    public void FillJobLevel()
    {
        Level = YearsOfExperience switch
        {
            <= 1 => new JobLevel { JobLevelId = JobLevelKey.Intern },
            <= 3 => new JobLevel { JobLevelId = JobLevelKey.Junior },
            _ => new JobLevel { JobLevelId = JobLevelKey.Senior },
        };
    }
}