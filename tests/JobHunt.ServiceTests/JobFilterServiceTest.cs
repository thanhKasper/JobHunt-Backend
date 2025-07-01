using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using JobHunt.Core.Domain.Entities;
using JobHunt.Core.Domain.RepositoryContracts;
using JobHunt.Core.DTO;
using JobHunt.Core.Helpers;
using JobHunt.Core.Services;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace JobHunt.ServiceTests;

public class JobFilterServiceTest
{
    private readonly JobFilterService _jobfilterService;
    private readonly IFixture _fixture;
    private readonly Mock<IJobFilterRepository> _jobFilterRepositoryMock;
    private readonly Mock<UserManager<JobHunter>> _userManagerMock;
    private readonly ITestOutputHelper _testOutputHelper;

    public JobFilterServiceTest(ITestOutputHelper testOutputHelper)
    {
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _testOutputHelper = testOutputHelper;

        _jobFilterRepositoryMock = new Mock<IJobFilterRepository>();
        IJobFilterRepository jobFilterRepo = _jobFilterRepositoryMock.Object;

        var store = new Mock<IUserStore<JobHunter>>();
        _userManagerMock = new Mock<UserManager<JobHunter>>(
            store.Object,
            null!, null!, null!, null!, null!, null!, null!, null!);

        _jobfilterService = new JobFilterService(jobFilterRepo, _userManagerMock.Object);
    }

    #region CreateNewJobFilterAsync
    [Fact]
    public async Task CreateNewJobFilter_EmptyArgument()
    {
        // Preparation
        JobFilterCreationRequest? filterReq = null;

        // Action
        async Task testAction()
        {
            await _jobfilterService.CreateNewJobFilterAsync(filterReq, Guid.NewGuid());
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
            async () => await _jobfilterService.CreateNewJobFilterAsync(req, Guid.NewGuid())
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
            async () => await _jobfilterService.CreateNewJobFilterAsync(req, Guid.NewGuid())
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
            async () => await _jobfilterService.CreateNewJobFilterAsync(req, Guid.NewGuid())
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

        _userManagerMock.Setup(repo => repo.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(_fixture.Create<JobHunter>());

        _jobFilterRepositoryMock.Setup(temp =>
            temp.AddJobFilterAsync(It.IsAny<JobFilter>(), It.IsAny<JobHunter>()))
            .ReturnsAsync(jobFilter);

        JobFilterResponseDetail? res = await _jobfilterService.CreateNewJobFilterAsync(req, Guid.NewGuid());

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

        _userManagerMock.Setup(repo => repo.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(_fixture.Create<JobHunter>());

        _jobFilterRepositoryMock.Setup(
            temp => temp.AddJobFilterAsync(It.IsAny<JobFilter>(), It.IsAny<JobHunter>()))
            .Returns<JobFilter?, JobHunter>((jobfilter, user) =>
            {
                return Task.FromResult(jobfilter);
            });

        JobFilterResponseDetail? res = await _jobfilterService.CreateNewJobFilterAsync(req, Guid.NewGuid());

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

        _userManagerMock.Setup(repo => repo.FindByIdAsync(It.IsAny<string>()))
        .ReturnsAsync(_fixture.Create<JobHunter>());

        _jobFilterRepositoryMock.Setup(
            temp => temp.AddJobFilterAsync(It.IsAny<JobFilter>(), It.IsAny<JobHunter>()))
            .Returns<JobFilter?, JobHunter>((jobfilter, user) =>
            {
                return Task.FromResult(jobfilter);
            });

        JobFilterResponseDetail? res = await _jobfilterService.CreateNewJobFilterAsync(req, Guid.NewGuid());

        Assert.NotNull(res!.Level);
    }

    [Fact]
    public async Task CreateNewJobFilterAsync_RemoveDuplicateKeys()
    {
        JobFilterCreationRequest req = _fixture.Build<JobFilterCreationRequest>()
            .With(jf => jf.Occupation, "Information_Technology")
            .With(jf => jf.Level, "intern")
            .With(jf => jf.YearsOfExperience, 0)
            .With(jf => jf.TechnicalKnowledge, ["a", "a", "b", "b", "c"])
            .With(jf => jf.Tools, ["git", "git", "github", "docker", "Github"])
            .With(jf => jf.Languages, ["asd", "ASd", "ddd", "jk", "jk", "jk"])
            .With(jf => jf.SoftSkills, ["dd", "dd", "dd", "dd"])
            .Create();

        _userManagerMock.Setup(service => service.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(_fixture.Build<JobHunter>()
                .With(user => user.Id, Guid.NewGuid()).Create());

        _jobFilterRepositoryMock.Setup(repo =>
            repo.AddJobFilterAsync(It.IsAny<JobFilter>(), It.IsAny<JobHunter>()))
            .Returns<JobFilter?, JobHunter>((jobfilter, user) =>
            {
                return Task.FromResult(jobfilter);
            });

        JobFilterResponseDetail res = await _jobfilterService.CreateNewJobFilterAsync(req, Guid.NewGuid());
        res.TechnicalKnowledge.Should().HaveCount(3);
        res.Tools.Should().HaveCount(3);
        res.Languages.Should().HaveCount(3);
        res.SoftSkills.Should().HaveCount(1);
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
            await _jobfilterService.CreateNewJobFilterAsync(req, Guid.NewGuid());
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

        _userManagerMock.Setup(repo => repo.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(_fixture.Create<JobHunter>());

        _jobFilterRepositoryMock.Setup(temp => temp.AddJobFilterAsync(It.IsAny<JobFilter>(), It.IsAny<JobHunter>()))
            .ReturnsAsync(req.ToJobFilter());

        var res = await _jobfilterService.CreateNewJobFilterAsync(req, Guid.NewGuid());

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
            await _jobfilterService.CreateNewJobFilterAsync(req, Guid.NewGuid());
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
            await _jobfilterService.CreateNewJobFilterAsync(req, Guid.NewGuid());
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
            await _jobfilterService.CreateNewJobFilterAsync(req, Guid.NewGuid());
        });
    }
    #endregion

    #region GetAllJobFilterSimpleAsync
    [Fact]
    public async Task GetAllJobFilterSimple_EmptyList()
    {
        _jobFilterRepositoryMock.Setup(temp => temp.GetAllJobFiltersAsync())
            .ReturnsAsync([]);

        List<JobFilterResponseSimple> actualList = await _jobfilterService.GetAllJobFilterSimpleAsync();

        actualList.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllJobFilterSimple_GetFewJobFilters()
    {
        JobFilter jobFilter1 = _fixture.Build<JobFilter>()
            .With(temp => temp.MatchJobList, [])
            .Create();
        JobFilter jobFilter2 = _fixture.Build<JobFilter>()
            .With(temp => temp.MatchJobList, [])
            .Create();
        _jobFilterRepositoryMock.Setup(temp => temp.GetAllJobFiltersAsync())
            .ReturnsAsync([jobFilter1, jobFilter2]);

        int expectResult = 2;
        int actual = (await _jobfilterService.GetAllJobFilterSimpleAsync()).Count;


        actual.Should().Be(expectResult);
    }
    #endregion

    #region GetJobFilterDetailAsync
    [Fact]
    public async Task GetJobFilterDetail_EmptyArgument()
    {
        JobFilterResponseDetail expected = await _jobfilterService.GetJobFilterDetailAsync(null);
        expected.Id.Should().Be(Guid.Empty);
    }

    [Fact]
    public async Task GetJobFilterDetail_NotFoundJobFilterId()
    {
        _jobFilterRepositoryMock.Setup(temp => temp.FindOneJobFilterByIdAsync(Guid.NewGuid()))
            .ReturnsAsync(() => null);
        JobFilterResponseDetail expected = await _jobfilterService.GetJobFilterDetailAsync(Guid.NewGuid());
        expected.Id.Should().Be(Guid.Empty);
    }

    [Fact]
    public void GetJobFilterDetail_FoundJobFilter()
    {
        Guid randomGuid = Guid.NewGuid();
        _jobFilterRepositoryMock.Setup(temp => temp.FindOneJobFilterByIdAsync(randomGuid))
            .ReturnsAsync(_fixture
                .Build<JobFilter>()
                .With(temp => temp.MatchJobList, [])
                .With(temp => temp.JobFilterId, randomGuid)
                .Create()
            );

        Assert.NotNull(_jobfilterService.GetJobFilterDetailAsync(randomGuid));
    }
    #endregion

    #region DeleteJobFilterAsync
    [Fact]
    public async Task DeleteJobFilter_NonExistingJobFilterId()
    {
        _jobFilterRepositoryMock.Setup(temp => temp.FindOneJobFilterByIdAsync(Guid.NewGuid()))
            .ReturnsAsync(() => null);
        JobFilterResponseSimple res = await _jobfilterService.DeleteJobFilterAsync(Guid.NewGuid());
        res.Id.Should().Be(Guid.Empty);
    }

    [Fact]
    public async Task DeleteJobFilter_NonMatchJobFilterId()
    {
        Guid randomGuid = Guid.NewGuid();
        _jobFilterRepositoryMock.Setup(temp => temp.FindOneJobFilterByIdAsync(randomGuid))
            .ReturnsAsync(() => null);

        var res = await _jobfilterService.DeleteJobFilterAsync(randomGuid);
        res.Id.Should().Be(Guid.Empty);
    }

    [Fact]
    public void DeleteJobFilter_MatchJobFilterId()
    {
        Guid randomGuid = Guid.NewGuid();
        _jobFilterRepositoryMock.Setup(temp => temp.FindOneJobFilterByIdAsync(randomGuid))
            .ReturnsAsync(_fixture
                .Build<JobFilter>()
                .With(temp => temp.MatchJobList, [])
                .With(temp => temp.JobFilterId, randomGuid)
                .Create()
            );

        Assert.NotNull(_jobfilterService.DeleteJobFilterAsync(randomGuid));
    }


    #endregion
}
