using JobHunt.Core.ServiceContracts.JobAnalysisServiceContract;
using JobHunt.Core.Services.JobAnalysisUseCase.JobPostingAnalyzer.DTO;
using JobHunt.Core.Services.JobAnalysisUseCase.JobPostingAnalyzer.Interfaces;

namespace JobHunt.Core.Services.JobAnalysisUseCase;

public class JobAnalysisService(
    IJobExpectationRepository jobExpectationRepository,
    IJobPostingFormatter jobPostFormatter,
    IJobPostingAnalyzer jobPostAnalyzer) : IJobAnalysisFacade
{
    private readonly IJobExpectationRepository _jobExpectationRepo = jobExpectationRepository;
    private readonly IJobPostingFormatter _jobPostFormatter = jobPostFormatter;
    private readonly IJobPostingAnalyzer _jobPostAnalyzer = jobPostAnalyzer;

    
    public void AnalyseJobPosting(string rawJobPosting)
    {
        if (String.IsNullOrEmpty(rawJobPosting))
            throw new ArgumentException("Argument cannot be null or empty");

        JobPosting formattedJobPosting = _jobPostFormatter.FormatJobPosting(rawJobPosting);
    }
}