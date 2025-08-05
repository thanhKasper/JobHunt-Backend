using AutoFixture;
using FluentAssertions;
using JobHunt.Core.Domain.Entities;
using JobHunt.Core.Domain.RepositoryContracts;
using JobHunt.Core.ServiceContracts.JobAnalysisServiceContract;
using JobHunt.Core.Services.JobAnalysisUseCase;
using JobHunt.Core.Services.JobAnalysisUseCase.JobPostingAnalyzer.DTO;
using JobHunt.Core.Services.JobAnalysisUseCase.JobPostingAnalyzer.Interfaces;
using JobHunt.Core.Services.JobAnalysisUseCase.JobPostingNormalizer.Interface;
using Microsoft.Extensions.Logging;
using Moq;

namespace JobHunt.ServiceTests.JobPostAnalysisTests;

public class JobPostAnalysisTest
{
    private static readonly int SATISFIED_THRESHOLD = 80;

    private readonly IJobAnalysisFacade _jobAnalysisService;
    private readonly Mock<IJobPostingFormatter> _jobPostingFormatterMock;
    private readonly Mock<IJobFilterReadRepository> _jobExpectationRepoMock;
    private readonly Mock<IJobPostingAnalyzer> _jobAnalysingMock;
    private readonly Mock<IJobPostNormalizer> _jobPostNormalizerMock;
    private readonly Mock<IJobUpdateRepository> _jobUpdateRepositoryMock;
    private readonly Mock<ILogger<JobAnalysisInterfaceAdapter>> _jobAnalysisLogger;
    private readonly IFixture _fixture;


    public JobPostAnalysisTest()
    {
        _jobPostingFormatterMock = new Mock<IJobPostingFormatter>();
        _jobExpectationRepoMock = new Mock<IJobFilterReadRepository>();
        _jobAnalysingMock = new Mock<IJobPostingAnalyzer>();
        _jobPostNormalizerMock = new Mock<IJobPostNormalizer>();
        _jobUpdateRepositoryMock = new Mock<IJobUpdateRepository>();
        _jobAnalysisLogger = new Mock<ILogger<JobAnalysisInterfaceAdapter>>();
        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(behavior => _fixture.Behaviors.Remove(behavior));
        _jobAnalysisService = new Mock<JobAnalysisInterfaceAdapter>(
            _jobExpectationRepoMock.Object,
            _jobPostingFormatterMock.Object,
            _jobAnalysingMock.Object,
            _jobPostNormalizerMock.Object,
            _jobUpdateRepositoryMock.Object,
            _jobAnalysisLogger.Object)
            .Object;
    }


    [Fact]
    public void AnalyseJobPosting_EmptyOrNullArgument_ThrowArgumentException()
    {
        var testMethod = GetAnalyseJobPostingMethodWithArg("");

        testMethod.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void AnalyseJobPosting_FailToFormatRawJobPost_ThrowArgumentException()
    {
        var testMethod = GetAnalyseJobPostingMethodWithArg("In valid job post format");

        FailToFormatJobPosting();

        testMethod.Should().Throw<ArgumentException>().WithMessage("Fail To Format the Job Post");
    }

    [Fact]
    public void AnalyseJobPosting_GetEmptyTitleAndDescriptionProject_ShouldThrowException()
    {
        var emptyTitleAndDescriptionProject = _fixture.Build<Project>()
            .With(p => p.ProjectTitle, string.Empty)
            .With(p => p.Description, string.Empty)
            .Create();

        var testMethod = GetAnalyseJobPostingMethodWithValidArg();

        SuccessfullyFormatJobPost();
        GetProjectsContainingEmptyTitleAndDescription();

        testMethod.Should().Throw<ArgumentException>()
            .WithMessage("Project title and description cannot be empty");

    }

    [Fact]
    public void AnalyseJobPosting_MatchingPercentageGreaterThan100_ShouldThrowOutOfRangeException()
    {
        var testMethod = GetAnalyseJobPostingMethodWithValidArg();
        SuccessfullyFormatJobPost();
        GetListOfJobFilters();
        JobPostMatchWithJobExpectation();
        ReturnInvalidMatchingPercentage(101);

        testMethod.Should().Throw<Exception>()
            .WithMessage("Matching percentage should be between 0% and 100%");
    }

    [Fact]
    public void AnalyseJobPosting_MatchingPercentageLessThan0_ShouldThrowOutOfRangeException()
    {
        var testMethod = GetAnalyseJobPostingMethodWithValidArg();
        SuccessfullyFormatJobPost();
        GetListOfJobFilters();
        JobPostMatchWithJobExpectation();
        ReturnInvalidMatchingPercentage(-1);

        testMethod.Should().Throw<Exception>()
            .WithMessage("Matching percentage should be between 0% and 100%");
    }

    private void ReturnInvalidMatchingPercentage(int invalidValue)
    {
        _jobAnalysingMock.Setup(mock => mock.CalculateMatchingPercentageBetweenJobPostAndJobSeeker(
            It.IsAny<JobPosting>(), It.IsAny<JobSeekerProfile>()))
            .Returns(invalidValue);
    }

    private void JobPostMatchWithJobExpectation()
    {
        _jobAnalysingMock.Setup(mock => mock.IsJobPostingMatchWithJobSeekerExpectation(
            It.IsAny<JobPosting>(), It.IsAny<UserJobFilter>()))
            .Returns(true);
    }

    private void GetListOfJobFilters()
    {
        _jobExpectationRepoMock.Setup(mock => mock.GetAllJobFiltersAsync())
            .ReturnsAsync(_fixture.CreateMany<JobFilter>().ToList());
    }

    private void SetUserBeSuitableForJobPost()
    {
        _jobAnalysingMock.Setup(mock => mock.CalculateMatchingPercentageBetweenJobPostAndJobSeeker(
            It.IsAny<JobPosting>(), It.IsAny<JobSeekerProfile>()))
            .Returns(SATISFIED_THRESHOLD);
    }

    private void GetProjectsContainingEmptyTitleAndDescription()
    {
        _jobExpectationRepoMock.Setup(mock => mock.GetAllJobFiltersAsync())
            .ReturnsAsync(_fixture.Build<JobFilter>()
            .With(jf => jf.JobFilterOwner, _fixture.Build<JobHunter>()
                .With(jh => jh.Projects, new List<Project> { _fixture.Build<Project>()
                    .With(p => p.ProjectTitle, string.Empty)
                    .With(p => p.Description, string.Empty)
                    .Create() })
                .Create())
            .CreateMany().ToList());
    }

    private Action GetAnalyseJobPostingMethodWithValidArg()
    {
        return GetAnalyseJobPostingMethodWithArg("A valid raw job post");
    }

    private Action GetAnalyseJobPostingMethodWithArg(string rawJobPost)
    {
        return () => { _jobAnalysisService.AnalyseJobPosting(rawJobPost); };
    }

    private void FailToFormatJobPosting()
    {
        _jobPostingFormatterMock.Setup(method => method.FormatJobPosting(It.IsAny<string>()))
            .Throws(new ArgumentException("Fail To Format the Job Post"));
    }

    private void SuccessfullyFormatJobPost()
    {
        _jobPostingFormatterMock.Setup(method => method.FormatJobPosting(It.IsAny<string>()))
            .Returns(
                _fixture.Create<JobPosting>()
            );
    }

}