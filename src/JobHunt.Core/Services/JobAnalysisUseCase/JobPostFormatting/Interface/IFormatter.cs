using JobHunt.Core.Services.JobAnalysisUseCase.JobPostingAnalyzer.DTO;

namespace JobHunt.Core.Services.JobAnalysisUseCase.JobPostFormatting.Interface;

public interface IFormatter
{
    JobPosting FormatJobPosting(string job); 
}

