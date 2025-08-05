namespace JobHunt.Core.Services.JobAnalysisUseCase.JobPostingAnalyzer.DTO;

public class UserJobFilter
{
    public int? YearsOfExperience { get; set; }
    public List<string> TechnicalKnowledge { get; set; } = [];
    public List<string> Tools { get; set; } = [];
    public List<string> Technologies { get; set; } = [];
    public List<string> SoftSkills { get; set; } = [];
    public List<string> Languages { get; set; } = [];
    public string? WorkingLocation { get; set; }
}