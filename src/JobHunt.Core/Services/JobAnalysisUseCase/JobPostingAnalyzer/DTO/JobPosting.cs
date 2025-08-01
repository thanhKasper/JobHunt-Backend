namespace JobHunt.Core.Services.JobAnalysisUseCase.JobPostingAnalyzer.DTO;

public class JobPosting
{
    string Title { get; set; } = null!;
    string Industry { get; set; } = null!;
    List<string> Requirements { get; set; } = [];
    List<string> Responsibilities { get; set; } = [];
    string WorkingLocation { get; set; } = null!;
}