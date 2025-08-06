using JobHunt.Core.Domain.Entities;

namespace JobHunt.Core.Services.JobAnalysisUseCase.JobPostingAnalyzer.DTO;

public class JobSeekerExpectation
{
    public int? YearsOfExperience { get; set; }
    public List<string> TechnicalKnowledge { get; set; } = [];
    public List<string> Tools { get; set; } = [];
    public List<string> Technologies { get; set; } = [];
    public List<string> SoftSkills { get; set; } = [];
    public List<string> Languages { get; set; } = [];
    public string? WorkingLocation { get; set; }


    public static JobSeekerExpectation ToJobSeekerExpectation(JobFilter jobFilter)
    {
        return new JobSeekerExpectation()
        {
            YearsOfExperience = jobFilter.YearsOfExperience,
            WorkingLocation = jobFilter.Location,
            TechnicalKnowledge = jobFilter.SpecializedKnowledges.Select(knowledge => knowledge.Knowledge!).ToList(),
            Tools = jobFilter.Tools.Select(tool => tool.ToolName!).ToList(),
            SoftSkills = jobFilter.SoftSkills.Select(skill => skill.SoftSkillName!).ToList(),
            Technologies = jobFilter.Technologies.Select(tech => tech.TechnologyName!).ToList(),
            Languages = jobFilter.Languages.Select(lang => lang.CommunicationLanguage + " - " + lang.Certification).ToList(),
        };
    }
}