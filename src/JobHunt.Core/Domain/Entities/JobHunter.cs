using System.ComponentModel.DataAnnotations;
using JobHunt.Core.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace JobHunt.Core.Domain.Entities;

public class JobHunter : IdentityUser<Guid>
{
    // Token related fields
    public string? RefreshToken { get; set; } = string.Empty;
    public DateTime? RefreshTokenExpirationDateTime { get; set; }

    // Business information
    [MaxLength(128)]
    [Required]
    public string? FullName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    [MaxLength(5000)]
    public string? AboutMe { get; set; }
    [MaxLength(128)]
    public string? Address { get; set; }
    [MaxLength(64)]
    public string? University { get; set; }
    [MaxLength(128)]
    public string? WorkingEmail { get; set; }

    // Navigational Properties
    public Major Major { get; set; } = null!;
    public Education Education { get; set; } = null!;
    public List<Project> Projects { get; set; } = [];
    public List<JobFilter> JobFilters { get; set; } = [];
    public List<Achievement> Awards { get; set; } = [];
}