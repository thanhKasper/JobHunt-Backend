using System.ComponentModel.DataAnnotations;

namespace JobHunt.Core.Domain.Entities;


public class Company
{
    [Key]
    public Guid? CompanyId { get; set; }
    [MaxLength(64)]
    [Required]
    public string? CompanyName { get; set; }
    [MaxLength(64)]
    public string? CompanyIconUrl { get; set; }
    public byte[]? CompanyIconImage { get; set; }
    [MaxLength(128)]
    [Required]
    public string? CompanyAddress { get; set; }
    [MaxLength(256)]
    public string? CompanyWebSite { get; set; }
    public List<Job> PostedJobs { get; } = [];
}