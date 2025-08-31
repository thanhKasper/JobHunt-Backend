using JobHunt.Core.Domain.Entities;
using JobHunt.Core.Domain.RepositoryContracts;
using JobHunt.Core.Services.JobAnalysisUseCase.JobPostingAnalyzer.DTO;
using JobHunt.Core.Services.JobAnalysisUseCase.JobPostingNormalizer.CompanySynchronizer;
using JobHunt.Core.Services.JobAnalysisUseCase.JobPostingNormalizer.Interface;
using Microsoft.Extensions.FileProviders;
using System.Diagnostics;

namespace JobHunt.Core.Services.JobAnalysisUseCase.JobPostingNormalizer
{
    public class JobPostNormalizer : IJobPostNormalizer
    { 
        ICompanySynchronizer _companySynchronizer;

        public JobPostNormalizer(
            ICompanySynchronizer companySynchronizer) 
        { 
            _companySynchronizer = companySynchronizer;
        }

        public Job NormalizeJobPost(JobPosting jobPost)
        {
            Company company = _companySynchronizer.SynchronizeCompanyFromJobPost(jobPost);
            Job job = new()
            {
                JobId = Guid.NewGuid(),
                Company = company,
                JobTitle = jobPost.Title,
                WorkingLocation = jobPost.WorkingLocation,
                JobDetailUrl = jobPost.JobPostUrl,
                PublishedDate = jobPost.PostDate
            };
            return job;
        }
    }
}
