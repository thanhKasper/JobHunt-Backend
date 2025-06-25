using System.ComponentModel.DataAnnotations;

namespace JobHunt.Core.Domain.Entities;

public class Job
{
    [Key]
    public Guid JobId { get; set; }
    [MaxLength(256)]
    [Required]
    public string? JobTitle { get; set; }
    public List<string>? MatchingRequirements { get; set; }
    public int? CompatiblePercentage { get; set; }
    public string? WorkingLocation { get; set; }
    [Required]
    [MaxLength(256)]
    public string? JobDetailUrl { get; set; }
    [Required]
    public DateTime? PublishedDate { get; set; }
    [Required]
    public DateTime? ExpiredDate { get; set; }
    public Company Company { get; set; } = null!;
    public JobFilter JobFilter { get; set; } = null!;
}