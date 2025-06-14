namespace JobHunt.Core.DTO;

public class JobResponse
{
    public string? JobTitle { get; set; }
    public string? JobLevel { get; set; }
    public string? Requirements { get; set; }
    public List<int>? ExpRange { get; set; }
    public string? JobPostUrl { get; set; }
    public DateTime? PostedDate { get; set; }
    public string? Company { get; set; }
    public string? CompanyIconUrl { get; set; }
    public string? CompanyUrl { get; set; }
    public string? CompanyAddress { get; set; }
    public string? JobFilterId { get; set; }
}