using AutoFixture;
using FluentAssertions;
using JobHunt.Core.ServiceContracts.JobAnalysisServiceContract;
using JobHunt.Core.Services.JobAnalysisUseCase.JobPostingAnalyzer;
using JobHunt.Core.Services.JobAnalysisUseCase.JobPostingAnalyzer.DTO;
using JobHunt.Core.Services.JobAnalysisUseCase.JobPostingAnalyzer.Interfaces;
using Moq;
using Xunit.Abstractions;

namespace JobHunt.ServiceTests.JobPostAnalysisTests
{
    public class JobPostAnalyserTest
    {
        private const int MIN_VALID_RESULT = 0;
        private const int MAX_VALID_RESULT = 100;

        private readonly IJobPostingAnalyzer _jobPostingAnalyzer;
        private readonly Mock<IJobPostingAnalyzingTool> _jobPostingAnalyzingToolMock;
        private readonly IFixture _fixture;
        private readonly ITestOutputHelper _outputHelper;
        public JobPostAnalyserTest(ITestOutputHelper outputHelper)
        {
            _fixture = new Fixture();
            _jobPostingAnalyzingToolMock = new Mock<IJobPostingAnalyzingTool>();
            _jobPostingAnalyzer = new Mock<JobPostingAnalyzer>(
                _jobPostingAnalyzingToolMock.Object
            ).Object;

            _outputHelper = outputHelper;
        }

        [Fact]
        public void IsJobPostingMatchWithJobSeekerExpectation_WhenJobPostMatchesExpectation_ShouldReturnTrue()
        {
            SetupJobFilterMatchingResult(80);
            var jobSeekerExpectation = GetOneJobSeekerExpectationt();
            bool expectedResult = MatchingJobSeekerExpectation(jobSeekerExpectation);

            expectedResult.Should().BeTrue();
        }
        private void SetupJobFilterMatchingResult(int returnValue)
        {
            _jobPostingAnalyzingToolMock
                .Setup(mock => mock.ComputeJobExpectationAlignment(It.IsAny<JobSeekerExpectation>()))
                .Returns(returnValue);
        }

        private JobSeekerExpectation GetOneJobSeekerExpectationt()
        {
            return _fixture.Create<JobSeekerExpectation>();
        }

        private bool MatchingJobSeekerExpectation(JobSeekerExpectation expectation)
        {
            return _jobPostingAnalyzer.IsJobSeekerExpectationMatch(expectation);
        }

        [Fact]
        public void IsJobPostingMatchWithJobSeekerExpectation_WhenJobPostNotMatchsExpectation_ShouldReturnFalse()
        {
            SetupJobFilterMatchingResult(50);
            var jobSeekerExpectation = GetOneJobSeekerExpectationt();
            bool expectedResult = MatchingJobSeekerExpectation(jobSeekerExpectation);

            expectedResult.Should().BeFalse();
        }

        [Fact]
        public void IsJobSeekerQualified_WhenSuccess_ShouldReturnValidResult()
        {
            int expectResult = CalculateCompatibilityBetweenJobPostAndJobSeeker();

            expectResult.Should().BeInRange(MIN_VALID_RESULT, MAX_VALID_RESULT);
        }

        private int CalculateCompatibilityBetweenJobPostAndJobSeeker()
        {
            var jobSeeker = GetOneJobSeekerAggregate();

            SetupJobSeekerProfileMatchingResult(80);
            SetupJobSeekerProjectsMatchingResult(75);
            SetupJobSeekerExperiencesMatchingResult(90);

            int result = _jobPostingAnalyzer.ComputeJobSeekerCompatibility(jobSeeker);

            //_outputHelper.WriteLine($"Job Seeker Profile Match Score: {result}");

            return result;
        }

        private JobSeekerAggregate GetOneJobSeekerAggregate()
        {
            return _fixture.Create<JobSeekerAggregate>();
        }

        private void SetupJobSeekerProfileMatchingResult(int matchingResult)
        {
            _jobPostingAnalyzingToolMock.Setup(mock => mock.ComputeJobSeekerProfileMatchScore(
                It.IsAny<JobSeekerProfile>()
            )).Returns(matchingResult);
        }

        private void SetupJobSeekerProjectsMatchingResult(int matchingResult)
        {
            _jobPostingAnalyzingToolMock.Setup(mock => mock.ComputeProjectsRelevance(
                It.IsAny<List<SelfProject>>()
            )).Returns(matchingResult);
        }

        private void SetupJobSeekerExperiencesMatchingResult(int matchingResult)
        {
            _jobPostingAnalyzingToolMock.Setup(mock => mock.ComputeExperiencesSuitability(
                It.IsAny<List<JobSeekerExperience>>()
            )).Returns(matchingResult);
        }
    }
}
