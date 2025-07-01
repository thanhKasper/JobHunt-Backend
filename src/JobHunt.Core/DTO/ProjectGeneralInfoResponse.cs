namespace JobHunt.Core.DTO;

public class ProjectGeneralInfoResponse
{
    public int TotalProjects { get; set; }
    public int TotalCompleteProjects { get; set; }
    public int TotalUsedTools { get; set; }
    public int TotalRoles { get; set; }
    public List<string> MostUsedTech { get; set; } = [];
}