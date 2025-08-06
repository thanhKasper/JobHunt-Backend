using JobHunt.Core.Domain.Entities;

namespace JobHunt.Core.Services.JobAnalysisUseCase.JobPostingAnalyzer.DTO
{
    public class JobSeekerExperience
    {
        public string WorkingCompany { get; set; } = null!;
        public List<string> Achievements { get; set; } = [];


        public static JobSeekerExperience ToJobSeekerExperience(WorkingExperience jobExperience)
        {
            return new JobSeekerExperience
            {
                WorkingCompany = jobExperience.CompanyName,
                Achievements = jobExperience.Achievements.Select(a => a.Achievement!).ToList()
            };
        }

        public static List<JobSeekerExperience> ToJobSeekerExperienceList(List<WorkingExperience> jobExperiences)
        {
            return jobExperiences.Select(ToJobSeekerExperience).ToList();
        }
    }
}
