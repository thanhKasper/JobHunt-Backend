using System.ComponentModel.DataAnnotations;
using JobHunt.Core.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace JobHunt.Core.Domain.Entities;

public class JobHunter : IdentityUser<Guid>
{
    // Token related fields
    [Required]
    public string? RefreshToken { get; set; } = string.Empty;
    [Required]
    public DateTime? RefreshTokenExpirationDateTime { get; set; }

    // Business information
    [MaxLength(128)]
    [Required]
    public string? FullName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    [MaxLength(512)]
    public string? AboutMe { get; set; }
    [MaxLength(64)]
    public string? Address { get; set; }
    [Required]
    public Education Education { get; set; } = ValueObjects.Education.None;
    [MaxLength(64)]
    public string? University { get; set; }
    [Required]
    public Major Major { get; set; } = ValueObjects.Major.None;

    [MaxLength(128)]
    public string? WorkingEmail { get; set; }
    public List<string>? Awards { get; set; }

    // Navigational Property
    public List<Project>? Projects { get; set; }
    public List<JobFilter> JobFilters { get; set; } = [];
}