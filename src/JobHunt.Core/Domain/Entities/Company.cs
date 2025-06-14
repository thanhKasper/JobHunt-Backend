using System.ComponentModel.DataAnnotations;

namespace JobHunt.Core.Domain.Entities;


public class Company
{
    [Key]
    public Guid? CompanyId { get; set; }
    [MaxLength(64)]
    public string? CompanyName { get; set; }
    public string? CompanyIconUrl { get; set; }
    public byte[]? CompanyIconImage { get; set; }
    public string? CompanyAddress { get; set; }
    public string? CompanyWebSite { get; set; }
    public List<Job> PostedJobs { get; } = [];
}