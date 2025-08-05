using JobHunt.Core.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace JobHunt.Core.Domain.Entities
{
    public class WorkingExperience
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Position { get; set; } = null!;
        [Required]
        [MaxLength(200)]
        public string CompanyName { get; set; } = null!;
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public JobHunter JobHunter { get; set; } = null!;
        public List<WorkingAchievement> Achievements { get; set; } = [];

    }
}
