using JobHunt.Core.Services.JobAnalysisUseCase.JobPostingAnalyzer.DTO;

namespace JobHunt.Core.Services.JobAnalysisUseCase.JobPostingAnalyzer.Interfaces;

public interface IJobPostingAnalyzer
{
    int JobMatchingPercentage(JobPosting jobPost);
}