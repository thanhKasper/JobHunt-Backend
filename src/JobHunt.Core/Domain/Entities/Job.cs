using System.ComponentModel.DataAnnotations;

namespace JobHunt.Core.Domain.Entities;

public class Job
{
    [Key]
    public Guid JobId { get; set; }
    [MaxLength(256)]
    [Required]
    public string? JobTitle { get; set; }
    public List<string>? Requirements { get; set; }
    public Company Company { get; set; } = null!;
    [Required]
    public DateTime? PublishedDate { get; set; }
    [Required]
    [MaxLength(256)]
    public string? JobDetailUrl { get; set; }
    public JobFilter JobFilter { get; set; } = null!;
    [Required]
    public DateTime? ExpiredDate { get; set; }
}