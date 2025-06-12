using System.ComponentModel.DataAnnotations;
using JobHunt.Core.CustomValidationAttributes;
using JobHunt.Core.Domain.Entities;
using JobHunt.Core.Domain.ValueObjects;

namespace JobHunt.Core.DTO;

public class JobFilterCreationRequest : IValidatableObject
{
    [Required]
    public string? FilterTitle { get; set; }

    [Required(ErrorMessage = "{0} cannot be empty")]
    [JobFieldValidation]
    public string? Occupation { get; set; }

    [JobLevelValidation]
    public string? Level { get; set; }

    [Range(0, 20, ErrorMessage = "{0} must be within range ({1}, {2})")]
    [Display(Name = "Years of experience")]
    public int? YearsOfExperience { get; set; }

    public List<string>? TechnicalKnowledge { get; set; }

    public List<string>? Tools { get; set; }

    public List<string>? SoftSkills { get; set; }
    
    public List<string>? Languages { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Level == null && !YearsOfExperience.HasValue)
        {
            yield return new ValidationResult(
                $"{nameof(Level)} and {nameof(YearsOfExperience)} cannot be both empty",
                [nameof(Level), nameof(YearsOfExperience)]
            );
        }
    }

    public JobFilter ToJobFilter()
    {
        DateTime currentTime = DateTime.Now;
        return new JobFilter()
        {
            JobFilterId = new Guid(),
            CreatedAt = currentTime,
            LastUpdated = currentTime,
            FilterTitle = FilterTitle,
            Level = Enum.Parse<JobLevel>(Level!, true),
            Occupation = Enum.Parse<JobField>(Occupation!, true),
            SoftSkills = SoftSkills,
            TechnicalKnowledge = TechnicalKnowledge,
            Tools = Tools,
            YearsOfExperience = YearsOfExperience
        };
    }
}