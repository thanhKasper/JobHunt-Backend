using System.Threading.Tasks;
using AutoFixture;
using EntityFrameworkCoreMock;
using FluentAssertions;
using JobHunt.Core.Domain.Entities;
using JobHunt.Core.Domain.RepositoryContracts;
using JobHunt.Core.DTO;
using JobHunt.Core.Helpers;
using JobHunt.Core.Services;
using JobHunt.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace JobHunt.ServiceTests;

public class JobFilterServiceTest
{
    private readonly JobFilterService _jobfilterService;
    private readonly IFixture _fixture;
    private readonly Mock<IJobFilterRepository> _jobFilterRepositoryMock;
    private readonly ITestOutputHelper _testOutputHelper;

    public JobFilterServiceTest(ITestOutputHelper testOutputHelper)
    {
        _fixture = new Fixture();
        var initialJobFilters = new List<JobFilter>();
        _testOutputHelper = testOutputHelper;

        DbContextMock<ApplicationDbContext> dbContextMock =
            new(new DbContextOptionsBuilder<ApplicationDbContext>().Options);

        ApplicationDbContext dbContext = dbContextMock.Object;
        dbContextMock.CreateDbSetMock(temp => temp.JobFilters, initialJobFilters);

        _jobFilterRepositoryMock = new Mock<IJobFilterRepository>();
        IJobFilterRepository jobFilterRepo = _jobFilterRepositoryMock.Object;

        _jobfilterService = new JobFilterService(jobFilterRepo);
    }

    #region CreateNewJobFilter
    [Fact]
    public async Task CreateNewJobFilter_EmptyArgument()
    {
        // Preparation
        JobFilterCreationRequest? filterReq = null;

        // Action
        async Task testAction()
        {
            await _jobfilterService.CreateNewJobFilter(filterReq);
        }

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(testAction);
    }

    [Fact]
    public async Task CreateNewJobFilter_EmptyJobFilterName()
    {
        JobFilterCreationRequest req = _fixture
                .Build<JobFilterCreationRequest>()
                .With(temp => temp.FilterTitle, string.Empty)
                .With(temp => temp.Occupation, "Information_Technology")
                .With(temp => temp.Level, "intern")
                .With(temp => temp.YearsOfExperience, 0)
                .Create();
        JobFilter jobFilter = req.ToJobFilter();

        await Assert.ThrowsAsync<ArgumentException>(
            async () => await _jobfilterService.CreateNewJobFilter(req)
        );
    }

    [Fact]
    public async Task CreateNewJobFitler_EmptyJobLevelAndYearsOfExperience()
    {
        JobFilterCreationRequest? req = _fixture
            .Build<JobFilterCreationRequest>()
            .With(temp => temp.Level, () => null)
            .With(temp => temp.YearsOfExperience, () => null)
            .Create();

        await Assert.ThrowsAsync<ArgumentException>(
            async () => await _jobfilterService.CreateNewJobFilter(req)
        );
    }

    [Fact]
    public async Task CreateNewJobFilter_EmptyOccupation()
    {
        JobFilterCreationRequest? req = _fixture
            .Build<JobFilterCreationRequest>()
            .With(temp => temp.YearsOfExperience, () => null)
            .With(temp => temp.Occupation, () => null)
            .Create();

        await Assert.ThrowsAsync<ArgumentException>(
            async () => await _jobfilterService.CreateNewJobFilter(req)
        );
    }

    [Fact]
    public async Task CreateNewJobFilter_ProperJobFilterCreation()
    {
        JobFilterCreationRequest? req = _fixture
            .Build<JobFilterCreationRequest>()
            .With(temp => temp.Level, "intern")
            .With(temp => temp.YearsOfExperience, 0)
            .With(temp => temp.Occupation, "Information_Technology")
            .Create();

        JobFilter jobFilter = req.ToJobFilter();

        _jobFilterRepositoryMock.Setup(temp => temp.AddJobFilter(It.IsAny<JobFilter>()))
            .ReturnsAsync(jobFilter);

        JobFilterResponseDetail? res = await _jobfilterService.CreateNewJobFilter(req);

        _testOutputHelper.WriteLine("Expect:");
        _testOutputHelper.WriteLine(jobFilter.ToJobFilterResponseDetail().ToString());

        _testOutputHelper.WriteLine("Actual:");
        _testOutputHelper.WriteLine(res?.ToString());

        Assert.Equal<JobFilterResponseDetail>(jobFilter.ToJobFilterResponseDetail(), res);
    }

    [Fact]
    public async Task CreateNewJobFilter_AutoCreateJobLevelWhenYearsOfExperienceIsProvided()
    {
        JobFilterCreationRequest? req = _fixture
            .Build<JobFilterCreationRequest>()
            .With(temp => temp.Occupation, "information_technology")
            .With(temp => temp.YearsOfExperience, 3)
            .With(temp => temp.Level, () => null)
            .Create();

        _jobFilterRepositoryMock.Setup(temp => temp.AddJobFilter(It.IsAny<JobFilter>()))
            .ReturnsAsync(req.ToJobFilter());

        JobFilterResponseDetail? res = await _jobfilterService.CreateNewJobFilter(req);

        Assert.NotNull(res!.Level);
    }

    [Fact]
    public async Task CreateNewJobFilter_AutoCreateYearsOfExperienceWhenJobLevelIsProvided()
    {
        JobFilterCreationRequest? req = _fixture
            .Build<JobFilterCreationRequest>()
            .With(temp => temp.Occupation, "information_technology")
            .With(temp => temp.YearsOfExperience, () => null)
            .With(temp => temp.Level, "junior")
            .Create();

        _jobFilterRepositoryMock.Setup(temp => temp.AddJobFilter(It.IsAny<JobFilter>()))
            .ReturnsAsync(req.ToJobFilter());

        JobFilterResponseDetail? res = await _jobfilterService.CreateNewJobFilter(req);

        Assert.NotNull(res!.Level);
    }

    [Fact]
    public void CreateNewJobFilter_RemoveDuplicateKeywords()
    {
        // Because using Moq, i already prepare the correct result, therefore i don't how whether my
        // utils function work correctly, hence, i will directly test the utils functions
        List<string> test = ["C#", "c#", "Java", "Powerpoint", "powerpoint"];
        List<string> expect = ["C#", "JAVA", "POWERPOINT"];
        List<string> actual = Utils.RemoveKeywordDuplication(test);
        _testOutputHelper.WriteLine("Expect: {0}", Utils.ToStringArray<string>(expect));
        _testOutputHelper.WriteLine("Actual: {0}", Utils.ToStringArray<string>(actual));
        Assert.Equal(expect.Count, actual.Count);
    }

    [Fact]
    public async Task CreateNewJobFilter_MismatchBetweenFresherAndExpYear()
    {
        JobFilterCreationRequest req = _fixture
            .Build<JobFilterCreationRequest>()
            .With(temp => temp.YearsOfExperience, 1)
            .With(temp => temp.Level, "fresher")
            .Create();

        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await _jobfilterService.CreateNewJobFilter(req);
        });
    }

    [Fact]
    public async Task CreateNewJobFilter_MatchBetweenJuniorAndExpYear()
    {
        JobFilterCreationRequest req = _fixture
            .Build<JobFilterCreationRequest>()
            .With(temp => temp.Occupation, "information_technology")
            .With(temp => temp.YearsOfExperience, 3)
            .With(temp => temp.Level, "junior")
            .Create();

        _jobFilterRepositoryMock.Setup(temp => temp.AddJobFilter(It.IsAny<JobFilter>()))
            .ReturnsAsync(req.ToJobFilter());

        var res = await _jobfilterService.CreateNewJobFilter(req);

        Assert.NotNull(res);
    }

    [Fact]
    public async Task CreateNewJobFilter_MismatchBetweenOtherLevelAndExpYear()
    {
        JobFilterCreationRequest req = _fixture
            .Build<JobFilterCreationRequest>()
            .With(temp => temp.YearsOfExperience, 1)
            .With(temp => temp.Level, "senior")
            .Create();

        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await _jobfilterService.CreateNewJobFilter(req);
        });
    }

    [Fact]
    public async Task CreateNewJobFilter_IncorrectJobLevelFormat()
    {
        JobFilterCreationRequest req = _fixture
            .Build<JobFilterCreationRequest>()
            .With(temp => temp.YearsOfExperience, () => null)
            .With(temp => temp.Level, "freher")
            .Create();

        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await _jobfilterService.CreateNewJobFilter(req);
        });
    }

    [Fact]
    public async Task CreateNewJobFilter_IncorrectOccupationFormat()
    {
        JobFilterCreationRequest req = _fixture
            .Build<JobFilterCreationRequest>()
            .With(temp => temp.YearsOfExperience, () => null)
            .With(temp => temp.Level, "freher")
            .Create();

        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await _jobfilterService.CreateNewJobFilter(req);
        });
    }
    #endregion

    #region GetAllJobFilterSimple
    [Fact]
    public async Task GetAllJobFilterSimple_EmptyList()
    {
        _jobFilterRepositoryMock.Setup(temp => temp.GetAllJobFilters())
            .ReturnsAsync([]);

        List<JobFilterResponseSimple> actualList = await _jobfilterService.GetAllJobFilterSimple();

        actualList.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllJobFilterSimple_GetFewJobFilters()
    {
        JobFilter jobFilter1 = _fixture.Create<JobFilter>();
        JobFilter jobFilter2 = _fixture.Create<JobFilter>();
        _jobFilterRepositoryMock.Setup(temp => temp.GetAllJobFilters())
            .ReturnsAsync([jobFilter1, jobFilter2]);

        int expectResult = 2;
        int actual = (await _jobfilterService.GetAllJobFilterSimple()).Count;


        actual.Should().Be(expectResult);
    }
    #endregion

    #region GetJobFilterDetail
    [Fact]
    public async Task GetJobFilterDetail_EmptyArgument()
    {
        JobFilterResponseDetail expected = await _jobfilterService.GetJobFilterDetail(null);
        expected.Id.Should().Be(Guid.Empty);
    }

    [Fact]
    public async Task GetJobFilterDetail_NotFoundJobFilterId()
    {
        _jobFilterRepositoryMock.Setup(temp => temp.FindOneJobFilterById(Guid.NewGuid()))
            .ReturnsAsync(() => null);
        JobFilterResponseDetail expected = await _jobfilterService.GetJobFilterDetail(Guid.NewGuid());
        expected.Id.Should().Be(Guid.Empty);
    }

    [Fact]
    public void GetJobFilterDetail_FoundJobFilter()
    {
        Guid randomGuid = Guid.NewGuid();
        _jobFilterRepositoryMock.Setup(temp => temp.FindOneJobFilterById(randomGuid))
            .ReturnsAsync(_fixture
                .Build<JobFilter>()
                .With(temp => temp.JobFilterId, randomGuid)
                .Create()
            );

        Assert.NotNull(_jobfilterService.GetJobFilterDetail(randomGuid));
    }
    #endregion

    #region DeleteJobFilter
    [Fact]
    public async Task DeleteJobFilter_NonExistingJobFilterId()
    {
        _jobFilterRepositoryMock.Setup(temp => temp.FindOneJobFilterById(Guid.NewGuid()))
            .ReturnsAsync(() => null);
        JobFilterResponseSimple res = await _jobfilterService.DeleteJobFilter(Guid.NewGuid());
        res.Id.Should().Be(Guid.Empty);
    }

    [Fact]
    public async Task DeleteJobFilter_NonMatchJobFilterId()
    {
        Guid randomGuid = Guid.NewGuid();
        _jobFilterRepositoryMock.Setup(temp => temp.FindOneJobFilterById(randomGuid))
            .ReturnsAsync(() => null);

        var res = await _jobfilterService.DeleteJobFilter(randomGuid);
        res.Id.Should().Be(Guid.Empty);
    }

    [Fact]
    public void DeleteJobFilter_MatchJobFilterId()
    {
        Guid randomGuid = Guid.NewGuid();
        _jobFilterRepositoryMock.Setup(temp => temp.FindOneJobFilterById(randomGuid))
            .ReturnsAsync(_fixture
                .Build<JobFilter>()
                .With(temp => temp.JobFilterId, randomGuid)
                .Create()
            );

        Assert.NotNull(_jobfilterService.DeleteJobFilter(randomGuid));
    }


    #endregion
}
