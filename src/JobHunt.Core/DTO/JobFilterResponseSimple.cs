using JobHunt.Core.Domain.Entities;

namespace JobHunt.Core.DTO;

public class JobFilterResponseSimple
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Occupation { get; set; }
    public bool? IsStarred { get; set; }
    public bool? IsActive { get; set; }
    public int? TotalJobs { get; set; }
    public DateTime? CreatedAt { get; set; }
}

public static class JobFilterConverterExtension
{
    public static JobFilterResponseSimple ToJobFilterResponseSimple(this JobFilter jobFilter)
    {
        return new()
        {
            Id = jobFilter.JobFilterId,
            CreatedAt = jobFilter.CreatedAt,
            Occupation = jobFilter.Occupation.VietNameseName,
            Title = jobFilter.FilterTitle,
            IsActive = jobFilter.IsActive,
            IsStarred = jobFilter.IsStarred,
            TotalJobs = jobFilter.JobsCount
        };
    }

    public static List<JobFilterResponseSimple> ToJobFilterResponseSimpleList(this IEnumerable<JobFilter> jobFilters)
    {
        return jobFilters.Select(jobfilter => jobfilter.ToJobFilterResponseSimple()).ToList();
    }
}