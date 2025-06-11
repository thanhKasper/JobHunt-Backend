namespace JobHunt.Core.DTO;

public class JobFilterResponseSimple
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Occupation { get; set; }
    public DateTime? CreatedAt { get; set; }
}