using AutoFixture;
using EntityFrameworkCoreMock;
using JobHunt.Core.Domain.Entities;
using JobHunt.Core.Domain.RepositoryContracts;
using JobHunt.Core.DTO;
using JobHunt.Core.Services;
using JobHunt.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moq;

namespace JobHunt.ServiceTests;

public class JobFilterServiceTest
{
    private readonly JobFilterService _jobfilterService;
    private readonly IFixture _fixture;
    private readonly Mock<IJobFilterRepository> _jobFilterRepositoryMock;

    public JobFilterServiceTest()
    {
        _fixture = new Fixture();
        var initialJobFilters = new List<JobFilter>();
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
            .Create();

        JobFilter jobFilter = req.ToJobFilter();

        _jobFilterRepositoryMock.Setup(temp => temp.AddJobFilter(It.IsAny<JobFilter>()))
            .ReturnsAsync(jobFilter);

        JobFilterResponseDetail? res = await _jobfilterService.CreateNewJobFilter(req);

        Assert.Equal<JobFilterResponseDetail>(jobFilter.ToJobFilterResponseDetail(), res);
    }

    [Fact]
    public async Task CreateNewJobFilter_AutoCreateJobLevelWhenYearsOfExperienceIsProvided()
    {
        JobFilterCreationRequest? req = _fixture
            .Build<JobFilterCreationRequest>()
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
            .With(temp => temp.YearsOfExperience, () => null)
            .With(temp => temp.Level, "junior")
            .Create();

        _jobFilterRepositoryMock.Setup(temp => temp.AddJobFilter(It.IsAny<JobFilter>()))
            .ReturnsAsync(req.ToJobFilter());

        JobFilterResponseDetail? res = await _jobfilterService.CreateNewJobFilter(req);

        Assert.NotNull(res!.Level);
    }

    [Fact]
    public async Task CreateNewJobFilter_RemoveDuplicateKeywordsInSoftSkillsField()
    {
        JobFilterCreationRequest req = _fixture
            .Build<JobFilterCreationRequest>()
            .With(temp => temp.YearsOfExperience, () => null)
            .With(temp => temp.Level, "junior")
            .With(temp => temp.SoftSkills, ["a", "A", "bc", "dd"])
            .Create();


        JobFilterResponseDetail? res = await _jobfilterService.CreateNewJobFilter(req);

        Assert.NotEqual(req.SoftSkills!.Count, res?.SoftSkills!.Count);
    }

    [Fact]
    public async Task CreateNewJobFilter_RemoveDuplicateKeywordsInToolsField()
    {
        JobFilterCreationRequest req = _fixture
            .Build<JobFilterCreationRequest>()
            .With(temp => temp.YearsOfExperience, () => null)
            .With(temp => temp.Level, "junior")
            .With(temp => temp.Tools, ["a", "A", "bc", "dd"])
            .Create();

        JobFilterResponseDetail? res = await _jobfilterService.CreateNewJobFilter(req);

        Assert.NotEqual(req.Tools!.Count, res!.Tools!.Count);
    }

    [Fact]
    public async Task CreateNewJobFilter_RemoveDuplicateKeywordsInLanguageField()
    {
        JobFilterCreationRequest req = _fixture
            .Build<JobFilterCreationRequest>()
            .With(temp => temp.YearsOfExperience, () => null)
            .With(temp => temp.Level, "junior")
            .With(temp => temp.Languages, ["a", "A", "bc", "dd"])
            .Create();

        JobFilterResponseDetail? res = await _jobfilterService.CreateNewJobFilter(req);

        Assert.NotEqual(req.Languages!.Count, res!.Languages!.Count);
    }

    [Fact]
    public async Task CreateNewJobFilter_RemoveDuplicateKeywordsInTechnicalKnowledgeField()
    {
        JobFilterCreationRequest req = _fixture
            .Build<JobFilterCreationRequest>()
            .With(temp => temp.YearsOfExperience, () => null)
            .With(temp => temp.Level, "junior")
            .With(temp => temp.TechnicalKnowledge, ["a", "A", "bc", "dd"])
            .Create();

        JobFilterResponseDetail? res = await _jobfilterService.CreateNewJobFilter(req);

        Assert.NotEqual(req.TechnicalKnowledge!.Count, res!.TechnicalKnowledge!.Count);
    }

    [Fact]
    public async Task CreateNewJobFilter_MismatchBetweenFresherAndExpYear()
    {
        JobFilterCreationRequest req = _fixture
            .Build<JobFilterCreationRequest>()
            .With(temp => temp.YearsOfExperience, 1)
            .With(temp => temp.Level, "fresher")
            .Create();

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
        {
            await _jobfilterService.CreateNewJobFilter(req);
        });
    }

    [Fact]
    public async Task CreateNewJobFilter_MatchBetweenJuniorAndExpYear()
    {
        JobFilterCreationRequest req = _fixture
            .Build<JobFilterCreationRequest>()
            .With(temp => temp.YearsOfExperience, 3)
            .With(temp => temp.Level, "junior")
            .Create();

        _jobFilterRepositoryMock.Setup(temp => temp.AddJobFilter(It.IsAny<JobFilter>()))
            .ReturnsAsync(req.ToJobFilter());

        var res = await _jobfilterService.CreateNewJobFilter(req);

        Assert.True(res!.Id != Guid.Empty);
    }

    [Fact]
    public async Task CreateNewJobFilter_MismatchBetweenOtherLevelAndExpYear()
    {
        JobFilterCreationRequest req = _fixture
            .Build<JobFilterCreationRequest>()
            .With(temp => temp.YearsOfExperience, 1)
            .With(temp => temp.Level, "senior")
            .Create();

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
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
    #endregion

    #region GetAllJobFilterSimple
    [Fact]
    public async Task GetAllJobFilterSimple_EmptyList()
    {
        _jobFilterRepositoryMock.Setup(temp => temp.GetAllJobFilters())
            .ReturnsAsync([]);

        List<JobFilterResponseSimple> actualList = await _jobfilterService.GetAllJobFilterSimple();

        Assert.Empty(actualList);
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


        Assert.Equal(actual, expectResult);
    }
    #endregion

    #region GetJobFilterDetail
    [Fact]
    public void GetJobFilterDetail_EmptyArgument()
    {
        Assert.Null(
            async () => await _jobfilterService.GetJobFilterDetail(null)
        );
    }

    [Fact]
    public void GetJobFilterDetail_NotFoundJobFilterId()
    {
        _jobFilterRepositoryMock.Setup(temp => temp.FindOneJobFilterById(Guid.NewGuid()))
            .ReturnsAsync(() => null);
        Assert.Null(
            async () => await _jobfilterService.GetJobFilterDetail(null)
        );
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
    public void DeleteJobFilter_NonExistingJobFilterId()
    {
        _jobFilterRepositoryMock.Setup(temp => temp.FindOneJobFilterById(Guid.NewGuid()))
            .ReturnsAsync(() => null);
        Assert.Null(
            async () => await _jobfilterService.DeleteJobFilter(null)
        );
    }

    [Fact]
    public void DeleteJobFilter_NonMatchJobFilterId()
    {
        Guid randomGuid = Guid.NewGuid();
        _jobFilterRepositoryMock.Setup(temp => temp.FindOneJobFilterById(randomGuid))
            .ReturnsAsync(() => null);

        Assert.Null(_jobfilterService.DeleteJobFilter(randomGuid));
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
