using AutoFixture;
using FluentAssertions;
using JobHunt.Core.Domain.Entities;
using JobHunt.Core.Domain.RepositoryContracts;
using JobHunt.Core.Services.JobAnalysisUseCase.CompanySynchronizer;
using JobHunt.Core.Services.JobAnalysisUseCase.JobPostingAnalyzer.DTO;
using JobHunt.Core.Services.JobAnalysisUseCase.JobPostingNormalizer;
using JobHunt.Core.Services.JobAnalysisUseCase.JobPostingNormalizer.Interface;
using Moq;

namespace JobHunt.ServiceTests.JobPostAnalysisTests
{
    public class JobPostNormalizationTest
    {
        private readonly IJobPostNormalizer _jobPostNormalizer;
        private readonly Mock<ICompanySynchronizer> _companySynchronizerMock;
        private readonly Fixture _fixture;

        public JobPostNormalizationTest()
        {
            _fixture = new Fixture();

            _companySynchronizerMock = new Mock<ICompanySynchronizer>();
            _jobPostNormalizer = new JobPostNormalizer(
                _companySynchronizerMock.Object
            );
        }

        [Fact]
        public void NormalizeJobPost_WhenNormalizeJobPosting_ShouldReturnJob()
        {
            var jobPost = CreateJobPosting();
            LinkCompanyFromJobPost(jobPost);
            var result = _jobPostNormalizer.NormalizeJobPost(jobPost);

            result.Should().NotBeNull();
            result.Company.Should().NotBeNull();
        }

        private JobPosting CreateJobPosting()
        {
            return _fixture.Create<JobPosting>();
        }

        private void LinkCompanyFromJobPost(JobPosting jobPost)
        {
            _companySynchronizerMock.Setup(mock => mock.SynchronizeCompanyFromJobPost(jobPost))
                .Returns(_fixture.Create<Company>());
        }
    }
}
