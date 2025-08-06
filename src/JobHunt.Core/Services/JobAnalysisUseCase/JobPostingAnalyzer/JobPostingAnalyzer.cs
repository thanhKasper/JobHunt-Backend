using JobHunt.Core.ServiceContracts.JobAnalysisServiceContract;
using JobHunt.Core.Services.JobAnalysisUseCase.JobPostingAnalyzer.DTO;
using JobHunt.Core.Services.JobAnalysisUseCase.JobPostingAnalyzer.Interfaces;

namespace JobHunt.Core.Services.JobAnalysisUseCase.JobPostingAnalyzer
{
    public class JobPostingAnalyzer(IJobPostingAnalyzingTool jobAnalysingTool) : IJobPostingAnalyzer
    {
        private const int JOB_FILTER_MATCHING_MINIMUM_VALUE = 80; 

        private readonly IJobPostingAnalyzingTool _jobAnalysingTool = jobAnalysingTool;
        private JobPosting jobPost = null!;

        public bool IsJobSeekerExpectationMatch(JobSeekerExpectation expectation)
        {
            int matchingPercentage = _jobAnalysingTool.ComputeJobExpectationAlignment(expectation);
            if (matchingPercentage >= JOB_FILTER_MATCHING_MINIMUM_VALUE)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int ComputeJobSeekerCompatibility(JobSeekerAggregate jobSeeker)
        {
            int profileMatchScore = _jobAnalysingTool.ComputeJobSeekerProfileMatchScore(jobSeeker.Profile);
            int projectsRelevanceScore = _jobAnalysingTool.ComputeProjectsRelevance(jobSeeker.SelfProjects);
            int experiencesSuitabilityScore = _jobAnalysingTool.ComputeExperiencesSuitability(jobSeeker.Experiences);

            int totalScore = ComputeFinalResult(profileMatchScore, projectsRelevanceScore, experiencesSuitabilityScore);

            return totalScore;

        }

        private int ComputeFinalResult(int profileScore, int projectScore, int experienceScore)
        {
            float PROFILE_WEIGHT = 0.2f;
            float PROJECT_WEIGHT = 0.4f;
            float EXPERIENCE_WEIGHT = 0.4f;

            if (projectScore == 0)
            {
                EXPERIENCE_WEIGHT = 0.8f;
                return (int)(profileScore * PROFILE_WEIGHT + experienceScore * EXPERIENCE_WEIGHT);
            }
            else if (experienceScore == 0)
            {
                PROJECT_WEIGHT = 0.8f;
                return (int)(profileScore * PROFILE_WEIGHT + projectScore * PROJECT_WEIGHT);
            }
            else
            {
                return (int)(profileScore * PROFILE_WEIGHT + projectScore * PROJECT_WEIGHT + experienceScore * EXPERIENCE_WEIGHT);
            }
        }

        public void SetJobPosting(JobPosting jobPost)
        {
            this.jobPost = jobPost;
            _jobAnalysingTool.SetJobPosting(jobPost);
        }
    }
}
