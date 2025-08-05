namespace JobHunt.Core.Services.JobAnalysisUseCase.JobPostingAnalyzer.DTO;

public class JobPosting
{
    string JobPostUrl { get; set; } = null!;
    DateTime PostDate { get; set; }
    DateTime ExpiredDate { get; set; }
    string Title { get; set; } = null!;
    int YearOfExperience { get; set; }
    string JobLevel { get; set; } = null!;
    string Industry { get; set; } = null!;
    string CompanyName { get; set; } = null!;
    string WorkingLocation { get; set; } = null!;
    string CompanyWebsite { get; set; } = null!;
    List<string> Requirements { get; set; } = [];
    List<string> Responsibilities { get; set; } = [];
}