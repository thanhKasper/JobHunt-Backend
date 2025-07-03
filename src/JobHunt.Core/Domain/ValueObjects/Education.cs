using System.ComponentModel.DataAnnotations;
using JobHunt.Core.Domain.Entities;

namespace JobHunt.Core.Domain.ValueObjects;

public enum EducationKey
{
    None = 0,
    HighSchool = 1,
    AssociateDegree = 2,
    BachelorDegree = 3,
    MasterDegree = 4,
    DoctorateDegree = 5
}

public class Education
{
    [Key]
    public EducationKey EducationId { get; set; }

    [Required]
    [MaxLength(64)]
    public string? VietNameseName { get; set; }

    public List<JobHunter> JobHunters { get; set; } = [];
}