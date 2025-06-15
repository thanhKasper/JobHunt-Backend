using System.ComponentModel.DataAnnotations;

namespace JobHunt.Core.Domain.Entities;

public class JobHunter
{
    [Key]
    public Guid JobHunterId { get; set; }
    [MaxLength(128)]
    [Required]
    public string? FullName { get; set; }
    public DateTime? DateOfBirth { get; set; }

    // Navigational Property
    public Profile? Profile { get; set; }
    public List<Project>? Projects { get; set; }
}