using JobHunt.Core.Domain.Entities;
using JobHunt.Core.Services.JobAnalysisUseCase.JobPostingAnalyzer.DTO;

namespace JobHunt.Core.Services.JobAnalysisUseCase.CompanySynchronizer
{
    public interface ICompanySynchronizer
    {
        Company SynchronizeCompanyFromJobPost(JobPosting jobPost);
    }
}
