namespace JobHunt.Core.Services.JobAnalysisUseCase.JobPostingAnalyzer.DTO;

public class SelfProject
{
    public string Title { get; set; } = null!;
    public List<string> TechStack { get; set; } = [];
    public List<string> Features { get; set; } = [];
    public List<string> Roles { get; set; } = [];
}