using System.ComponentModel.DataAnnotations;
using JobHunt.Core.Domain.Entities;

namespace JobHunt.Core.Domain.ValueObjects;

public class ProjectFeature
{
    [Key]
    public Guid? Id { get; set; }
    [MaxLength(256)]
    public string? Feature { get; set; }
    public Project Project { get; set; } = null!;
}