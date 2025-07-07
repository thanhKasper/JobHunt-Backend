using System.ComponentModel.DataAnnotations;
using JobHunt.Core.Domain.Entities;

namespace JobHunt.Core.Domain.ValueObjects;

public class Tool
{
    [Key]
    public Guid? Id { get; set; }
    [MaxLength(32)]
    public string? ToolName { get; set; }

    public JobFilter JobFilter { get; set; } = null!;
}
