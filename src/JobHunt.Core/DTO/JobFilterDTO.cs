using System.ComponentModel.DataAnnotations;
using JobHunt.Core.Utils;

namespace JobHunt.Core.DTO;

public class JobFilterDTO : IValidatableObject
{
    [Required(ErrorMessage = "{0} cannot be empty")]
    public string? Occupation { get; set; }
    [RegularExpression("^(intern|fresher|junior|staff|senior|lead|manager|director)$",
    ErrorMessage = "Invalid syntax, please check again. Valid syntax is: (intern|fresher|junior|staff|senior|lead|manager|director)")]
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
}