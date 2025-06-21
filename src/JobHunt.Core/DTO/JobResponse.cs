using JobHunt.Core.Domain.Entities;

namespace JobHunt.Core.DTO;

public class JobResponse
{
    public string? JobTitle { get; set; }
    public List<string>? MatchingRequirements { get; set; }
    public string? JobPostUrl { get; set; }
    public DateTime? PostedDate { get; set; }
    public string? Company { get; set; }
    public string? CompanyIconUrl { get; set; }
    public string? CompanyUrl { get; set; }
    public string? CompanyAddress { get; set; }
    public Guid? JobFilterId { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is not JobResponse other)
            return false;

        return JobTitle == other.JobTitle &&
               MatchingRequirements?.SequenceEqual(other.MatchingRequirements ?? []) == true && // need reconsider carefully
               JobPostUrl == other.JobPostUrl &&
               PostedDate == other.PostedDate &&
               Company == other.Company &&
               CompanyIconUrl == other.CompanyIconUrl &&
               CompanyUrl == other.CompanyUrl &&
               CompanyAddress == other.CompanyAddress &&
               JobFilterId == other.JobFilterId;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

public static class JobEntityConverterExtension
{
    public static JobResponse ToJobResponse(this Job job)
    {
        return new JobResponse
        {
            JobTitle = job.JobTitle,
            MatchingRequirements = job.MatchingRequirements,
            JobPostUrl = job.JobDetailUrl,
            PostedDate = job.PublishedDate,
            Company = job.Company?.CompanyName,
            CompanyIconUrl = job.Company?.CompanyIconUrl,
            CompanyUrl = job.Company?.CompanyWebSite,
            CompanyAddress = job.Company?.CompanyAddress,
            JobFilterId = job.JobFilter?.JobFilterId
        };
    }
    public static List<JobResponse> ToJobResponseList(this IEnumerable<Job> jobs)
    {
        return jobs.Select(job => job.ToJobResponse()).ToList();
    }
}