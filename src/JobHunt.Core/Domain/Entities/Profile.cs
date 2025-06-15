using System.ComponentModel.DataAnnotations;
using JobHunt.Core.Domain.ValueObjects;

namespace JobHunt.Core.Domain.Entities;

public class Profile
{
    [Key]
    public Guid ProfileId { get; set; }
    public string? AboutMe { get; set; }
    [MaxLength(64)]
    public string? Address { get; set; }
    [Required]
    public Education Education { get; set; } = ValueObjects.Education.None;
    [MaxLength(64)]
    public string? University { get; set; }
    [Required]
    public Major Major { get; set; } = ValueObjects.Major.None;
    [MaxLength(24)]
    public string? PhoneNumber { get; set; }
    [MaxLength(128)]
    public string? WorkingEmail { get; set; }
    public List<string>? Awards { get; set; }

    // Navigational Properties
    public JobHunter JobFinder { get; set; } = null!;
}