using JobHunt.Core.Domain.ValueObjects;

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

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var other = (ProfileResponse)obj;
        return FullName == other.FullName &&
               DateOfBirth == other.DateOfBirth &&
               WorkingEmail == other.WorkingEmail &&
               AboutMe == other.AboutMe &&
               Address == other.Address &&
               Education == other.Education &&
               University == other.University &&
               Major == other.Major &&
               PhoneNumber == other.PhoneNumber &&
               (Awards ?? new List<string>()).SequenceEqual(other?.Awards ?? new List<string>());
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
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
            Education = jobHunter.Education?.EducationId.ToString()
                ?? EducationKey.None.ToString(),
            University = jobHunter.University,
            Major = jobHunter.Major?.MajorId.ToString()
                ?? MajorKey.None.ToString(),
            PhoneNumber = jobHunter.PhoneNumber,
            Awards = jobHunter.Awards.Select(a => a.AchievementName!).ToList(),
        };
    }
}