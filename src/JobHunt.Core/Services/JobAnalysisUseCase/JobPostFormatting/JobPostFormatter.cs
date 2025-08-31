using JobHunt.Core.ServiceContracts.JobAnalysisServiceContract;
using JobHunt.Core.Services.JobAnalysisUseCase.JobPostingAnalyzer.DTO;
using JobHunt.Core.Services.JobAnalysisUseCase.JobPostFormatting.Interface;

namespace JobHunt.Core.Services.JobAnalysisUseCase.JobPostFormatting
{

    public class JobPostFormatter(
        IJobPostRawValidator validator,
        IRawJobPostParser rawParser,
        IJobPostContentParser contentParser) : IFormatter
    {
        private readonly IJobPostRawValidator _jobPostValidator = validator;
        private readonly IRawJobPostParser _rawJobPostParser = rawParser;
        private readonly IJobPostContentParser _jobPostContentParser = contentParser;
        public JobPosting FormatJobPosting(string job)
        {
            if (_jobPostValidator.ValidSyntax(job))
            {
                JobPosting jobPosting = new()
                {
                    JobPostUrl = _rawJobPostParser.GetJobPostUrl(job),
                    Industry = _rawJobPostParser.GetJobPostIndustry(job),
                    CompanyName = _rawJobPostParser.GetJobPostCompany(job),
                    CompanyWebsite = _rawJobPostParser.GetJobPostCompanyWebsite(job),
                    WorkingLocation = _rawJobPostParser.GetJobPostCompanyAddress(job)
                };

                AssignJobContent(jobPosting, _rawJobPostParser.GetRawContent(job));

                return jobPosting;
            }
            else 
            {
                throw new ArgumentException("Job posting format is invalid");
            }
        }

        private void AssignJobContent(JobPosting jobPost, string rawJobContent)
        {
            JobPostContent jobContent = _jobPostContentParser.ParseContent(rawJobContent);
            jobPost.Title = jobContent.Title;
            jobPost.Responsibilities = jobContent.Responsibilities;
            jobPost.Requirements = jobContent.Requirements;
            jobPost.YearOfExperience = jobContent.YearOfExperience;
            jobPost.JobLevel = jobContent.JobLevel.ToString();
        }

    }
}
