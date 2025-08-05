using JobHunt.Core.Services.JobAnalysisUseCase.JobPostingAnalyzer.DTO;

namespace JobHunt.Core.Services.JobAnalysisUseCase.JobPostingAnalyzer.Interfaces;

public interface IJobPostingAnalyzer
{
    bool IsJobPostingMatchWithJobSeekerExpectation(JobPosting jobPost, UserJobFilter userExpectation);
    int CalculateMatchingPercentageBetweenJobPostAndJobSeeker(JobPosting jobPost, JobSeekerProfile jobSeeker);
}