using AutoFixture;
using FluentAssertions;
using JobHunt.Core.Domain.Entities;
using JobHunt.Core.Domain.RepositoryContracts;
using JobHunt.Core.Services.JobAnalysisUseCase.CompanySynchronizer;
using JobHunt.Core.Services.JobAnalysisUseCase.JobPostingAnalyzer.DTO;
using Moq;

namespace JobHunt.ServiceTests.JobPostAnalysisTests
{
    public class CompanySynchronizerTest
    {
        private readonly CompanySynchronizerImp _companySynchronizer;
        private readonly Mock<ICompanyGetRepository> _companyGetRepoMock;
        private readonly Mock<ICompanyUpdateRepository> _companyUpdateRepoMock;
        private readonly Fixture _fixture;
        
        public CompanySynchronizerTest()
        {
            _companyGetRepoMock = new Mock<ICompanyGetRepository>();
            _companyUpdateRepoMock = new Mock<ICompanyUpdateRepository>();
            _fixture = new Fixture();

            _companySynchronizer = new CompanySynchronizerImp(
                _companyGetRepoMock.Object,
                _companyUpdateRepoMock.Object
            );
        }

        [Fact]
        public void SynchronizeCompanyFromJobPost_WhenNormal_ShouldReturnResult()
        {
            FoundCompanyInTheDataSource();
            SetupCompanyUpToDate();

            var jobPost = GetJobPost();
            var expected = _companySynchronizer.SynchronizeCompanyFromJobPost(jobPost);

            expected.Should().NotBeNull();
        }

        private void FoundCompanyInTheDataSource()
        {
            Company company = _fixture.Create<Company>();
            _companyGetRepoMock.Setup(mock => mock.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(company);
        }

        private void SetupCompanyUpToDate()
        {
            Company company = _fixture.Create<Company>();
            _companyGetRepoMock.Setup(mock => mock.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(company);
            _companyGetRepoMock.Setup(mock => mock.FindByWebsiteAsync(It.IsAny<string>()))
                .ReturnsAsync(company);
            _companyGetRepoMock.Setup(mock => mock.FindByAddressAsync(It.IsAny<string>()))
                .ReturnsAsync(company);
        }

        private JobPosting GetJobPost()
        {
            return _fixture.Create<JobPosting>();
        }
    }
}
