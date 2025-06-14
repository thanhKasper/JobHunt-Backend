using System.ComponentModel.DataAnnotations;

namespace JobHunt.Core.Domain.Entities;

public class Job
{
    [Key]
    public Guid JobId { get; set; }
    public string? JobTitle { get; set; }
    public List<string>? Requirements { get; set; }
    public Company Company { get; set; } = null!;
    public DateTime? PublishedDate { get; set; }
    public string? JobDetailUrl { get; set; }
    public JobFilter? JobFilter { get; set; } = null!;
    public DateTime? ExpiredDate { get; set; }
}