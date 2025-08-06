using JobHunt.Core.Domain.Entities;
using System.Net.NetworkInformation;

namespace JobHunt.Core.Services.JobAnalysisUseCase.JobPostingAnalyzer.DTO
{
    public class JobSeekerAggregate
    {
        public JobSeekerProfile Profile { get; set; } = null!;
        public List<SelfProject> SelfProjects { get; set; } = [];
        public List<JobSeekerExperience> Experiences { get; set; } = [];


        public static JobSeekerAggregate ToJobSeekerAggregate(JobHunter jobSeeker)
        {
            return new JobSeekerAggregate
            {
                Profile = JobSeekerProfile.ToJobSeekerProfile(jobSeeker),
                SelfProjects = SelfProject.ToSelfProjectList(jobSeeker.Projects),
                Experiences = JobSeekerExperience.ToJobSeekerExperienceList(jobSeeker.WorkingExperiences)
            };
        }
    }
}
