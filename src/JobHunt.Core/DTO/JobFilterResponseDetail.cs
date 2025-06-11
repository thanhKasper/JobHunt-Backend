namespace JobHunt.Core.DTO;

public class JobFilterResponseDetail : JobFilterResponseSimple
{
    public string? Level { get; set; }
    public int? YearsOfExperience { get; set; }
    public List<string>? TechnicalKnowledge { get; set; }
    public List<string>? Tools { get; set; }
    public List<string>? SoftSkills { get; set; }
    public List<string>? Languages { get; set; }
    public int? TotalJobMatch { get; set; }
    public List<JobResponse>? JobList { get; set; }
}