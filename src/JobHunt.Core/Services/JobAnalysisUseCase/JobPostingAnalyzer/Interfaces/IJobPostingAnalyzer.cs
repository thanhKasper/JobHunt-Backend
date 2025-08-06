using JobHunt.Core.Services.JobAnalysisUseCase.JobPostingAnalyzer.DTO;

namespace JobHunt.Core.Services.JobAnalysisUseCase.JobPostingAnalyzer.Interfaces;

public interface IJobPostingAnalyzer
{
    void SetJobPosting(JobPosting jobPost);
    bool IsJobSeekerExpectationMatch(JobSeekerExpectation expectation);
    int ComputeJobSeekerCompatibility(JobSeekerAggregate jobSeeker);
}