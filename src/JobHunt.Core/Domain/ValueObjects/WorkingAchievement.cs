using JobHunt.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace JobHunt.Core.Domain.ValueObjects
{
    public class WorkingAchievement
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string Achievement { get; set; } = null!;

        public WorkingExperience WorkingExperience { get; set; } = null!;
    }
}
