namespace JobHunt.Core.Domain.ValueObjects;


public class WorkingLocation
{
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? Other { get; set; } // Containing other information like address numger, street, etc.
}