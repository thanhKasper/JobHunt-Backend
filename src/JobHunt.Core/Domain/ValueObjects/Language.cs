using System.ComponentModel.DataAnnotations;
using JobHunt.Core.Domain.Entities;

namespace JobHunt.Core.Domain.ValueObjects;

public class Language
{
    [Key]
    public Guid? Id { get; set; }
    [MaxLength(64)]
    public string? CommunicationLanguage { get; set; }
    public JobFilter JobFilter { get; set; } = null!;
}