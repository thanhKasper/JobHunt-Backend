using JobHunt.Core.Domain.Entities;

namespace JobHunt.Core.Services.JobAnalysisUseCase.JobPostingAnalyzer.DTO;

public class JobPosting
{
    public string JobPostUrl { get; set; } = null!;
    public DateTime PostDate { get; set; }
    public DateTime ExpiredDate { get; set; }
    public string Title { get; set; } = null!;
    public int YearOfExperience { get; set; }
    public string JobLevel { get; set; } = null!;
    public string Industry { get; set; } = null!;
    public string CompanyName { get; set; } = null!;
    public string WorkingLocation { get; set; } = null!;
    public string CompanyWebsite { get; set; } = null!;
    public List<string> Requirements { get; set; } = [];
    public List<string> Responsibilities { get; set; } = [];


    public Company CreateComapny()
    {
        return new Company()
        {
            CompanyName = CompanyName,
            CompanyAddress = WorkingLocation,
            CompanyWebSite = CompanyWebsite,
        };
    }
}