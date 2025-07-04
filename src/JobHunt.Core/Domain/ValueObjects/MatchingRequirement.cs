using JobHunt.Core.Domain.Entities;

namespace JobHunt.Core.Domain.ValueObjects;

public class MatchingRequirement
{
    public Guid? Id { get; set; }
    public string? Requirement { get; set; }
    public Job Job { get; set; } = null!;
}