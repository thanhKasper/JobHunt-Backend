using JobHunt.Core.Domain.Entities;
using JobHunt.Core.Services.JobAnalysisUseCase.JobPostingAnalyzer.DTO;

namespace JobHunt.Core.Services.JobAnalysisUseCase.JobPostingNormalizer.Interface
{
    public interface IJobPostNormalizer
    {
        Job NormalizeJobPost(JobPosting jobPost);
    }
}
