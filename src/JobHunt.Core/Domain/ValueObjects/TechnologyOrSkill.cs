using System.ComponentModel.DataAnnotations;
using JobHunt.Core.Domain.Entities;

namespace JobHunt.Core.Domain.ValueObjects;

public class TechnologyOrSkill
{
    [Key]
    public Guid? Id { get; set; }
    [MaxLength(64)]
    public string? TechOrSkill { get; set; }
    public Project Project { get; set; } = null!;
}