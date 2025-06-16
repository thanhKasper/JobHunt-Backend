namespace JobHunt.Core.DTO;

public class ProfileResponse
{
    public string? FullName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? WorkingEmail { get; set; }
    public string? AboutMe { get; set; }
    public string? Address { get; set; }
    public string? Education { get; set; }
    public string? University { get; set; }
    public string? Major { get; set; }
    public string? PhoneNumber { get; set; }
    public List<string>? Awards { get; set; }
}

public static class ProfileResponseExtensions
{
    public static ProfileResponse ToProfileResponse(this Domain.Entities.JobHunter jobHunter)
    {
        return new ProfileResponse
        {
            FullName = jobHunter.FullName,
            DateOfBirth = jobHunter.DateOfBirth,
            WorkingEmail = jobHunter.WorkingEmail,
            AboutMe = jobHunter.AboutMe,
            Address = jobHunter.Address,
            Education = jobHunter.Education.ToString(),
            University = jobHunter.University,
            Major = jobHunter.Major.ToString(),
            PhoneNumber = jobHunter.PhoneNumber,
            Awards = jobHunter.Awards
        };
    }
}