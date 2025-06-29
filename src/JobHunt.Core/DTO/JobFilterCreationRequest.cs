using System.ComponentModel.DataAnnotations;
using JobHunt.Core.CustomValidationAttributes;
using JobHunt.Core.Domain.Entities;
using JobHunt.Core.Domain.ValueObjects;

namespace JobHunt.Core.DTO;

public class JobFilterCreationRequest : IValidatableObject
{
    [Required]
    public string? FilterTitle { get; set; }

    public bool? IsActive { get; set; }
    public bool? IsStarred { get; set; }

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
    public string? WorkingLocation { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (String.IsNullOrEmpty(Level) && !YearsOfExperience.HasValue)
        {
            yield return new ValidationResult(
                $"{nameof(Level)} and {nameof(YearsOfExperience)} cannot be both empty",
                [nameof(Level), nameof(YearsOfExperience)]
            );
        }


        if (Level != null && YearsOfExperience != null)
        {
            if ((Level.Equals("INTERN", StringComparison.CurrentCultureIgnoreCase) ||
                Level.Equals("FRESHER", StringComparison.CurrentCultureIgnoreCase)) &&
                YearsOfExperience >= 1)
            {
                yield return new ValidationResult(
                    $"{Level} cannot have experience greater or equal to 1",
                    [nameof(YearsOfExperience)]
                );
            }
            else if (
                Level.Equals("JUNIOR", StringComparison.CurrentCultureIgnoreCase) &&
                (YearsOfExperience > 3 || YearsOfExperience < 1))
            {
                yield return new ValidationResult(
                    $"Junior level should have experience between 1 and 3 inclusively",
                    [nameof(YearsOfExperience)]    
                );
            }
            else if (
                Level.Equals("MIDDLE", StringComparison.CurrentCultureIgnoreCase) &&
                (YearsOfExperience > 5 || YearsOfExperience <= 3))
            {
                yield return new ValidationResult(
                    $"Middle level should have experience more than 3 years until 5 years",
                    [nameof(YearsOfExperience)]
                );
            }
            else if (
                !Level.Equals("JUNIOR", StringComparison.CurrentCultureIgnoreCase) &&
                !Level.Equals("INTERN", StringComparison.CurrentCultureIgnoreCase) &&
                !Level.Equals("FRESHER", StringComparison.CurrentCultureIgnoreCase) &&
                YearsOfExperience < 5)
            {
                yield return new ValidationResult(
                    $"{Level} cannot have experience less than 5 years",
                    [nameof(YearsOfExperience)]    
                );
            }
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
            Level = Enum.TryParse(Level, true, out JobLevel res) ? res : null,
            Occupation = Enum.TryParse(Occupation, true, out JobField jobField) ? jobField : null,
            SoftSkills = SoftSkills,
            TechnicalKnowledge = TechnicalKnowledge,
            Tools = Tools,
            YearsOfExperience = YearsOfExperience,
            IsActive = IsActive,
            IsStarred = IsStarred,
            Location = WorkingLocation,
            Languages = Languages
        };
    }
}