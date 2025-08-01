using AutoFixture;
using FluentAssertions;
using JobHunt.Core.ServiceContracts.JobAnalysisServiceContract;
using JobHunt.Core.Services.JobAnalysisUseCase;
using JobHunt.Core.Services.JobAnalysisUseCase.JobPostingAnalyzer.DTO;
using JobHunt.Core.Services.JobAnalysisUseCase.JobPostingAnalyzer.Interfaces;
using Moq;

namespace JobHunt.ServiceTests.JobPostAnalysisTests;

public class JobPostAnalysisTest
{
    private readonly IJobAnalysisFacade _jobAnalysisService;
    private readonly Mock<IJobPostingFormatter> _jobPostingFormatterMock;
    private readonly Mock<IJobExpectationRepository> _jobExpectationMock;
    private readonly Mock<IJobPostingAnalyzer> _jobAnalysingMock;
    private readonly IFixture _fixture;
    public JobPostAnalysisTest()
    {
        _jobPostingFormatterMock = new Mock<IJobPostingFormatter>();
        _jobExpectationMock = new Mock<IJobExpectationRepository>();
        _jobAnalysingMock = new Mock<IJobPostingAnalyzer>();
        _fixture = new Fixture();
        _jobAnalysisService = new Mock<JobAnalysisService>(
            _jobExpectationMock.Object,
            _jobPostingFormatterMock.Object,
            _jobAnalysingMock.Object).Object;
    }

    [Fact]
    public void AnalyseJobPosting_EmptyOrNullArgument_ThrowArgumentException()
    {
        var testMethod = () => _jobAnalysisService.AnalyseJobPosting("");

        testMethod.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void AnalyseJobPosting_FailToFormatRawJobPost_ThrowArgumentException()
    {
        var testMethod = () => _jobAnalysisService.AnalyseJobPosting("formatter error job posting");

        FailToFormatJobPosting();

        testMethod.Should().Throw<ArgumentException>().WithMessage("Fail To Format the Job Post");
    }

    [Fact]
    public void AnalyseJobPosting_SuccessfullyFormatJobPost()
    {
        var testMethod = () => _jobAnalysisService.AnalyseJobPosting("successfully format the job");

        SuccessfullyFormatJobPost();

        testMethod.Should().NotThrow();
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