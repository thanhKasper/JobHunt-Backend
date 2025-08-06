using JobHunt.Core.Services.JobAnalysisUseCase.JobPostingAnalyzer.DTO;

namespace JobHunt.Core.ServiceContracts.JobAnalysisServiceContract;

public interface IJobPostingAnalyzingTool
{
    void SetJobPosting(JobPosting jobPost);
    int ComputeJobSeekerProfileMatchScore(JobSeekerProfile profile);
    int ComputeProjectsRelevance(List<SelfProject> projects);
    int ComputeExperiencesSuitability(List<JobSeekerExperience> experiences);
    int ComputeJobExpectationAlignment(JobSeekerExpectation jobExpectation);
}