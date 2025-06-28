using JobHunt.Core.Domain.Entities;
using JobHunt.Core.Helpers;

namespace JobHunt.Core.DTO;

public class JobFilterResponseDetail : JobFilterResponseSimple
{
    public string? Level { get; set; }
    public int? YearsOfExperience { get; set; }
    public List<string>? TechnicalKnowledge { get; set; }
    public List<string>? Tools { get; set; }
    public List<string>? SoftSkills { get; set; }
    public List<string>? Languages { get; set; }
    public string? WorkingLocation { get; set; }

    // These will be updated when Job Service is created
    // public int? TotalJobMatch { get; set; }
    // public List<JobResponse>? JobList { get; set; }

    public override string ToString()
    {
        return @$"JobFilterResponseDetail:
        Id: {Id}
        Title: {Title}
        Occupation: {Occupation}
        Level: {Level}
        YearsOfExperience: {YearsOfExperience}
        TechnicalKnowledge: {Utils.ToStringArray<string>(TechnicalKnowledge)}
        Tools: {Utils.ToStringArray<string>(Tools)}
        SoftSkill: {Utils.ToStringArray<string>(SoftSkills)}
        Languages: {Utils.ToStringArray<string>(Languages)}";
    }

    public override bool Equals(object? obj)
    {
        JobFilterResponseDetail? res = (JobFilterResponseDetail?)obj;
        if (res != null)
        {
            return this.ToString() == res.ToString();
        }
        return false;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
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
            IsActive = jobFilter.IsActive,
            IsStarred = jobFilter.IsStarred,
            YearsOfExperience = jobFilter.YearsOfExperience,
            WorkingLocation = jobFilter.Location
        };
    }
}