using System.ComponentModel.DataAnnotations;
using JobHunt.Core.Domain.Entities;
using JobHunt.Core.Domain.ValueObjects;

namespace JobHunt.Core.DTO;

public class ProfileRequest : IValidatableObject
{
    [Required]
    public Guid JobFinderId { get; set; }
    public string? FullName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? WorkingEmail { get; set; }
    public string? AboutMe { get; set; }
    public string? Address { get; set; }
    public string? Education { get; set; }
    public string? University { get; set; }
    public string? Major { get; set; }
    [Phone]
    public string? PhoneNumber { get; set; }
    public List<string>? Awards { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!Enum.TryParse<Education>(Education, true, out var educationValue))
        {
            yield return new ValidationResult("Invalid education level.", [nameof(Education)]);
        }

        if (!Enum.TryParse<Major>(Major, true, out var majorValue))
        {
            yield return new ValidationResult("Invalid major.", [nameof(Major)]);
        }
    }
    
    public JobHunter ToJobHunter()
    {
        return new JobHunter
        {
            JobHunterId = JobFinderId,
            FullName = FullName,
            DateOfBirth = DateOfBirth,
            AboutMe = AboutMe,
            WorkingEmail = WorkingEmail,
            Address = Address,
            Education = Enum.TryParse<Education>(Education, true, out var education) ?
                education :
                JobHunt.Core.Domain.ValueObjects.Education.None,
            University = University,
            Major = Enum.TryParse<Major>(Major, true, out var major) ?
                major :
                JobHunt.Core.Domain.ValueObjects.Major.None,
            PhoneNumber = PhoneNumber,
            Awards = Awards
            
        };
    }
}