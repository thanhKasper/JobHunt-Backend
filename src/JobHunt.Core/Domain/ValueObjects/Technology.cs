using System.ComponentModel.DataAnnotations;
using JobHunt.Core.Domain.Entities;

namespace JobHunt.Core.Domain.ValueObjects;

public class Technology
{
    [Key]
    public Guid? Id { get; set; }
    [MaxLength(64)]
    public string? TechnologyName { get; set; }
    public Project? Project { get; set; }
    public JobFilter? JobFilter { get; set; }
}