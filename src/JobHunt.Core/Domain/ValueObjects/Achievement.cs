using System.ComponentModel.DataAnnotations;
using JobHunt.Core.Domain.Entities;

namespace JobHunt.Core.Domain.ValueObjects;

public class Achievement
{
    [Key]
    public Guid? Id { get; set; }
    [MaxLength(128)]
    public string? AchievementName { get; set; }
    public JobHunter JobHunter { get; set; } = null!;
}