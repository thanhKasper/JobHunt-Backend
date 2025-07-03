using System.ComponentModel.DataAnnotations;
using JobHunt.Core.Domain.Entities;

namespace JobHunt.Core.Domain.ValueObjects;

public enum JobLevelKey
{
    Intern,
    Fresher,
    Junior,
    Senior,
    TeamLead,
    Manager,
    Director
}

public class JobLevel
{
    [Key]
    public JobLevelKey? JobLevelId { get; set; }

    [Required]
    [MaxLength(64)]
    public string? VietNameseName { get; set; }
    
    public List<JobFilter> JobFilters { get; set; } = [];
}