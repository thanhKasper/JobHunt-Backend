using JobHunt.Core.Domain.Entities;

namespace JobHunt.Core.DTO;

public class JobFilterResponseDetail : JobFilterResponseSimple
{
    public string? Level { get; set; }
    public int? YearsOfExperience { get; set; }
    public List<string>? TechnicalKnowledge { get; set; }
    public List<string>? Tools { get; set; }
    public List<string>? SoftSkills { get; set; }
    public List<string>? Languages { get; set; }

    // These will be updated when Job Service is created
    // public int? TotalJobMatch { get; set; }
    // public List<JobResponse>? JobList { get; set; }
}

public static class JobFilterResponseExtension
{
    public static JobFilterResponseDetail ToJobFilterResponseDetail(this JobFilter jobFilter)
    {
        return new JobFilterResponseDetail()
        {
            CreatedAt = jobFilter.CreatedAt,
            Id = jobFilter.JobFilterId,
            Languages = jobFilter.Languages,
            Level = jobFilter.Level.ToString(),
            Occupation = jobFilter.Occupation.ToString(),
            SoftSkills = jobFilter.SoftSkills,
            TechnicalKnowledge = jobFilter.TechnicalKnowledge,
            Title = jobFilter.FilterTitle,
            Tools = jobFilter.Tools,
            YearsOfExperience = jobFilter.YearsOfExperience
        };
    }
}