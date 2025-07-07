using System.ComponentModel.DataAnnotations;
using JobHunt.Core.Domain.Entities;

namespace JobHunt.Core.Domain.ValueObjects;

public class SoftSkill
{
    [Key]
    public Guid? Id { get; set; }
    [MaxLength(32)]
    public string? SoftSkillName { get; set; }
    public JobFilter JobFilter { get; set; } = null!;
}