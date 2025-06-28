namespace JobHunt.Core.DTO;

public class JobFilterListResponse
{
    public int TotalJobs { get; set; }
    public int TotalActiveJobs { get; set; }
    public List<JobFilterResponseSimple> JobFilters { get; set; } = [];
}