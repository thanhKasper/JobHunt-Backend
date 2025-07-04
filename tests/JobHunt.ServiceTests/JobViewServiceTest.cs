using AutoFixture;
using FluentAssertions;
using JobHunt.Core.Domain.Entities;
using JobHunt.Core.Domain.RepositoryContracts;
using JobHunt.Core.DTO;
using JobHunt.Core.ServiceContracts;
using JobHunt.Core.Services;
using Moq;
using Xunit.Abstractions;

namespace JobHunt.ServiceTests;

public class JobViewServiceTest
{
    // Use for auto generate testing object
    private readonly IFixture _fixture;
    // Used for mocking repository action
    private readonly Mock<IJobViewRepository> _jobViewRepoMock;
    private readonly IJobViewRepository _jobViewRepo;
    // Print test output result when necessary
    private readonly ITestOutputHelper _testOuputHelper;
    private readonly IJobViewService _jobViewService;

    public JobViewServiceTest(ITestOutputHelper outputHelper)
    {
        _fixture = new Fixture();
        _testOuputHelper = outputHelper;

        _jobViewRepoMock = new Mock<IJobViewRepository>();
        _jobViewRepo = _jobViewRepoMock.Object;
        _jobViewService = new JobViewService(_jobViewRepo);

    }

    #region GetAllMatchingJob
    [Fact]
    public async Task GetAllMatchingJob_EmptyJobList_ToBeSuccess()
    {
        _jobViewRepoMock.Setup(repoTemp => repoTemp.GetAllJobs())
            .ReturnsAsync([]);
        var actual = await _jobViewService.GetAllMatchingJobs();
        actual.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllMatchingJob_GetSomeJobs_ToBeSuccess()
    {
        List<Job> jobList = [
            _fixture.Build<Job>()
                .With(temp => temp.Company, null as Company)
                .With(temp => temp.JobFilter, null as JobFilter)
                .Create(),
            _fixture.Build<Job>()
                .With(temp => temp.Company, null as Company)
                .With(temp => temp.JobFilter, null as JobFilter)
                .Create(),
            _fixture.Build<Job>()
                .With(temp => temp.Company, null as Company)
                .With(temp => temp.JobFilter, null as JobFilter)
                .Create()
        ];
        _jobViewRepoMock.Setup(repo => repo.GetAllJobs())
            .ReturnsAsync(jobList);

        var actual = await _jobViewService.GetAllMatchingJobs();

        actual.Should().BeEquivalentTo(jobList.ToJobResponseList());
    }
    #endregion

    #region GetAllMatchingJobsBaseOnJobFilter
    [Fact]
    public async Task GetAllMatchingJobsBaseOnJobFilter_EmptyJobList_ToBeSuccess()
    {
        _jobViewRepoMock.Setup(repoTemp => repoTemp.GetAllJobsBaseOnJobFilterId(It.IsAny<Guid>()))
            .ReturnsAsync([]);
        var actual = await _jobViewService.GetAllMatchingJobsBaseOnJobFilter(Guid.NewGuid());
        actual.Should().BeEmpty();
    }
    [Fact]
    public async Task GetAllMatchingJobsBaseOnJobFilter_GetSomeJobs_ToBeSuccess()
    {
        List<Job> jobList = [
            _fixture.Build<Job>()
                .With(temp => temp.Company, null as Company)
                .With(temp => temp.JobFilter, null as JobFilter)
                .Create(),
            _fixture.Build<Job>()
                .With(temp => temp.Company, null as Company)
                .With(temp => temp.JobFilter, null as JobFilter)
                .Create(),
            _fixture.Build<Job>()
                .With(temp => temp.Company, null as Company)
                .With(temp => temp.JobFilter, null as JobFilter)
                .Create()
        ];
        _jobViewRepoMock.Setup(repo => repo.GetAllJobsBaseOnJobFilterId(It.IsAny<Guid>()))
            .ReturnsAsync(jobList);

        var actual = await _jobViewService.GetAllMatchingJobsBaseOnJobFilter(Guid.NewGuid());

        actual.Should().BeEquivalentTo(jobList.ToJobResponseList());
    }
    #endregion
}