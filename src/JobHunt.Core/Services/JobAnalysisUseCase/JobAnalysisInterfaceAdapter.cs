using JobHunt.Core.Domain.Entities;
using JobHunt.Core.Domain.RepositoryContracts;
using JobHunt.Core.Domain.ValueObjects;
using JobHunt.Core.ServiceContracts.JobAnalysisServiceContract;
using JobHunt.Core.Services.JobAnalysisUseCase.JobPostingAnalyzer.DTO;
using JobHunt.Core.Services.JobAnalysisUseCase.JobPostingAnalyzer.Interfaces;
using JobHunt.Core.Services.JobAnalysisUseCase.JobPostingNormalizer.Interface;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Logging;

namespace JobHunt.Core.Services.JobAnalysisUseCase;

public class JobAnalysisInterfaceAdapter(
    IJobFilterReadRepository jobExpectationRepository,
    IJobPostingFormatter jobPostFormatter,
    IJobPostingAnalyzer jobPostAnalyzer,
    IJobPostNormalizer jobPostNormalizer,
    IJobUpdateRepository _jobUpdateRepository,
    ILogger<JobAnalysisInterfaceAdapter> logger) : IJobAnalysisFacade
{

    private static readonly int MATCHING_PERCENTAGE_THRESHOLD = 70;

    private readonly IJobFilterReadRepository _jobExpectationRepo = jobExpectationRepository;
    private readonly IJobPostingFormatter _jobPostFormatter = jobPostFormatter;
    private readonly IJobPostingAnalyzer _jobPostAnalyzer = jobPostAnalyzer;
    private readonly IJobPostNormalizer _jobPostNormalizer = jobPostNormalizer;
    private readonly IJobUpdateRepository _jobUpdateRepository = _jobUpdateRepository;
    private readonly ILogger<JobAnalysisInterfaceAdapter> _logger = logger;


    public void AnalyseJobPosting(string rawJobPosting)
    {
        try
        {
            AnalyseJobPostingWithNoExceptionHandling(rawJobPosting);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "An error occurred while analysing the job posting: {Message}",
                ex.Message);
            throw;
        }

    }

    public void AnalyseJobPostingWithNoExceptionHandling(string rawJobPosting)
    {
        if (String.IsNullOrEmpty(rawJobPosting))
            throw new ArgumentException("Argument cannot be null or empty");

        JobPosting formattedJobPosting = _jobPostFormatter.FormatJobPosting(rawJobPosting);
        List<JobFilter> jobFilters = _jobExpectationRepo.GetAllJobFiltersAsync().Result;

        foreach (var jobfilter in jobFilters)
        {
            AnalyzeJobPostForEachJobFilter(formattedJobPosting, jobfilter);
        }
    }

    private void AnalyzeJobPostForEachJobFilter(JobPosting formattedJobPosting, JobFilter jobfilter)
    {
        JobSeekerProfile jobseeker = ToJobSeekerProfile(jobfilter.JobFilterOwner);
        UserJobFilter userJobExpectation = ToUserJobFilter(jobfilter);

        if (_jobPostAnalyzer.IsJobPostingMatchWithJobSeekerExpectation(formattedJobPosting, userJobExpectation))
        {
            int matchingPercentage = _jobPostAnalyzer
                .CalculateMatchingPercentageBetweenJobPostAndJobSeeker(formattedJobPosting, jobseeker);

            ValidateMatchingPercentage(matchingPercentage);

            if (SatisfyPercentageThreshold(matchingPercentage))
            {
                Job newJob = _jobPostNormalizer.NormalizeJobPost(formattedJobPosting);
                newJob.UpdateCompatiblePercentage(matchingPercentage);
                _jobUpdateRepository.AddNewJobAsync(newJob);
            }
        }
    }

    private static void ValidateMatchingPercentage(int matchingPercentage)
    {
        const int MININUM_MATCHING_PERCENTAGE = 0;
        const int MAXIMUM_MATCHING_PERCENTAGE = 100;

        if (matchingPercentage < MININUM_MATCHING_PERCENTAGE ||
            matchingPercentage > MAXIMUM_MATCHING_PERCENTAGE)
        {
            throw new Exception(
                "Matching percentage should be between 0% and 100%");
        }
    }

    private JobSeekerProfile ToJobSeekerProfile(JobHunter jobSeeker)
    {
        return new JobSeekerProfile()
        {
            Awards = jobSeeker.Achievements.Select(a => a.Achievement!).ToList(),
            Education = jobSeeker.Education.EducationId.ToString(),
            StudyMajor = jobSeeker.Major.MajorId.ToString(),
            SelfProjects = ToSelfProjectList(jobSeeker.Projects ?? [])
        };
    }

    private UserJobFilter ToUserJobFilter(JobFilter jobFilter)
    {
        return new UserJobFilter()
        {
            YearsOfExperience = jobFilter.YearsOfExperience,
            WorkingLocation = jobFilter.Location,
            TechnicalKnowledge = jobFilter.SpecializedKnowledges.Select(knowledge => knowledge.Knowledge!).ToList(),
            Tools = jobFilter.Tools.Select(tool => tool.ToolName!).ToList(),
            SoftSkills = jobFilter.SoftSkills.Select(skill => skill.SoftSkillName!).ToList(),
            Technologies = jobFilter.Technologies.Select(tech => tech.TechnologyName!).ToList(),
            Languages = jobFilter.Languages.Select(lang => lang.CommunicationLanguage + " - " + lang.Certification).ToList(),
        };
    }

    private static bool SatisfyPercentageThreshold(int matchingPercentage)
    {
        return matchingPercentage >= MATCHING_PERCENTAGE_THRESHOLD;
    }

    private List<SelfProject> ToSelfProjectList(List<Project> projects)
    {
        return projects.Select(ToSelfProject).ToList();
    }

    private SelfProject ToSelfProject(Project project)
    {
        if (string.IsNullOrEmpty(project.ProjectTitle) && 
            string.IsNullOrEmpty(project.Description))
        {
            throw new ArgumentException("Project title and description cannot be empty");
        }

        return new SelfProject()
        {
            Title = project.ProjectTitle ?? "",
            Description = project.Description ?? "",
            Features = project.Features.Select(feat => feat.Feature!).ToList(),
            Roles = project.Roles.Select(role => role.ProjectOwnerRole!).ToList(),
            TechStack = project.Technologies.Select(tech => tech.TechnologyName!).ToList(),
            Tools = project.Tools.Select(tool => tool.ToolName!).ToList(),
        };
    }

    
}