using JobHunt.Core.Domain.Entities;

namespace JobHunt.Core.Services.JobAnalysisUseCase.JobPostingAnalyzer.DTO;

public class JobSeekerProfile
{
    public string StudyMajor { get; set; } = null!;
    public string Education { get; set; } = null!;
    public List<string> Awards { get; set; } = [];

    public static JobSeekerProfile ToJobSeekerProfile(JobHunter jobSeeker)
    {
        return new JobSeekerProfile()
        {
            Awards = jobSeeker.Achievements.Select(a => a.Achievement!).ToList(),
            Education = jobSeeker.Education.EducationId.ToString(),
            StudyMajor = jobSeeker.Major.MajorId.ToString()
        };
    }
}