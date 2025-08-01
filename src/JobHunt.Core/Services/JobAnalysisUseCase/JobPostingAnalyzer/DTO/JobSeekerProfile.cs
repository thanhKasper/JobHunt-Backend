namespace JobHunt.Core.Services.JobAnalysisUseCase.JobPostingAnalyzer.DTO;

public class JobSeekerProfile
{
    public string StudyMajor { get; set; } = null!;
    public string Education { get; set; } = null!;
    public List<string> Awards { get; set; } = [];
    public List<SelfProject> SelfProjects { get; set; } = [];
}