using System.ComponentModel.DataAnnotations;
using JobHunt.Core.Domain.Entities;

namespace JobHunt.Core.Domain.ValueObjects;

public class Role
{
    [Key]
    public Guid? Id { get; set; }
    [MaxLength(32)]
    public string? ProjectOwnerRole { get; set; }
    public Project Project { get; set; } = null!;
}