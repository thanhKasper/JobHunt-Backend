using JobHunt.Core.Services.JobAnalysisUseCase.JobPostingAnalyzer.DTO;

namespace JobHunt.Core.ServiceContracts.JobAnalysisServiceContract;

public interface IJobPostingFormatter
{
    JobPosting FormatJobPosting(string job); 
}

