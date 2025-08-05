using System.ComponentModel.DataAnnotations;
using JobHunt.Core.Domain.Entities;

namespace JobHunt.Core.Domain.ValueObjects;

public class PersonalAchievement
{
    [Key]
    public Guid? Id { get; set; }
    [MaxLength(128)]
    public string? Achievement { get; set; }
    public JobHunter JobHunter { get; set; } = null!;
}