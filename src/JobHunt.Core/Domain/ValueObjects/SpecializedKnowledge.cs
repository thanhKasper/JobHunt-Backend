using System.ComponentModel.DataAnnotations;
using JobHunt.Core.Domain.Entities;

namespace JobHunt.Core.Domain.ValueObjects;

public class SpecializedKnowledge
{
    [Key]
    public Guid? Id { get; set; }
    [MaxLength(32)]
    public string? Knowledge { get; set; }
    public JobFilter JobFilter { get; set; } = null!;
}