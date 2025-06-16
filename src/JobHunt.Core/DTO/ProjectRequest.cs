using System.ComponentModel.DataAnnotations;
using JobHunt.Core.Domain.Entities;

namespace JobHunt.Core.DTO;

public class ProjectRequest : IValidatableObject
{
    [Required(ErrorMessage = "Project title is required")]
    [MaxLength(200, ErrorMessage = "Project title cannot exceed 200 characters")]
    public string ProjectTitle { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime? StartDate { get; set; }

    [DataType(DataType.Date)]
    public DateTime? EndDate { get; set; }

    [MaxLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
    public string? Description { get; set; }

    public List<string>? Role { get; set; }

    public List<string>? TechnologiesOrSkills { get; set; }

    public List<string>? Features { get; set; }

    [Url(ErrorMessage = "Please enter a valid URL")]
    [MaxLength(500, ErrorMessage = "Link cannot exceed 500 characters")]
    public string? ProjectLink { get; set; }

    // Custom validation to ensure EndDate is after StartDate
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (StartDate.HasValue && EndDate.HasValue && EndDate < StartDate)
        {
            yield return new ValidationResult(
                "End date must be after start date",
                new[] { nameof(EndDate) });
        }
    }

    public Project ToProject()
    {
        return new Project()
        {
            ProjectTitle = ProjectTitle,
            Description = Description,
            EndDate = EndDate,
            Features = Features,
            Role = Role,
            ProjectLink = ProjectLink,
            StartDate = StartDate,
            TechnologiesOrSkills = TechnologiesOrSkills,
        };
    }
}