using JobHunt.Core.Services.JobAnalysisUseCase.JobPostingAnalyzer.DTO;

namespace JobHunt.Core.Services.JobAnalysisUseCase.JobPostingAnalyzer.Interfaces;

public interface IJobPostingAnalyzingTool
{
    public int JobSeekerProfileMatchingPercentage(JobSeekerProfile profile);
    public int JobFilterMatchingPercentage(UserJobFilter jobfilter);
}