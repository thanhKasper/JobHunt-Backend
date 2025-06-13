using JobHunt.Core.Domain.Entities;
using JobHunt.Core.Domain.ValueObjects;

namespace JobHunt.Core.DTO;

public class JobFilterResponseSimple
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Occupation { get; set; }
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
            Occupation = jobFilter.Occupation.ToString(),
            Title = jobFilter.FilterTitle
        };
    }
}