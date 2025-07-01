using AutoFixture;
using FluentAssertions;
using JobHunt.Core.Domain.Entities;
using JobHunt.Core.Domain.RepositoryContracts;
using JobHunt.Core.DTO;
using JobHunt.Core.ServiceContracts;
using JobHunt.Core.Services;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace JobHunt.ServiceTests;

public class ProjectServiceTest
{
    private readonly Mock<IProjectRepository> _projectRepositoryMock;
    private readonly IProjectService _projectService;
    private readonly Mock<IProfileRepository> _profileRepositoryMock;
    private readonly IFixture _fixture;
    private readonly Mock<UserManager<JobHunter>> _userManagerMock;
    public ProjectServiceTest()
    {
        _projectRepositoryMock = new Mock<IProjectRepository>();
        _profileRepositoryMock = new Mock<IProfileRepository>();
        var mockUserStore = new Mock<IUserStore<JobHunter>>();
        _userManagerMock = new Mock<UserManager<JobHunter>>(
            mockUserStore.Object,
            null!, // IOptions<IdentityOptions>
            null!, // IPasswordHasher<JobHunter>
            null!, // IEnumerable<IUserValidator<JobHunter>>
            null!, // IEnumerable<IPasswordValidator<JobHunter>>
            null!, // ILookupNormalizer
            null!, // IdentityErrorDescriber
            null!, // IServiceProvider
            null!  // ILogger<UserManager<JobHunter>>
        );
        _projectService = new ProjectService(
            _projectRepositoryMock.Object,
            _profileRepositoryMock.Object,
            _userManagerMock.Object
        );

        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
               .ToList()
               .ForEach(b => _fixture.Behaviors.Remove(b));

        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        _fixture.Customize<JobHunter>(
            c => c
            .Without(user => user.Projects)
            .Without(user => user.JobFilters));
    }

    #region CreateProjectAsync
    [Fact]
    public async Task CreateProjectAsync_CreateNewProject_ShouldBeSuccessfull()
    {
        var projectOwner = _fixture.Build<JobHunter>()
            // .With(po => po.Projects, [])
            // .With(po => po.JobFilters, [])
            .Create();

        var projectRequest = _fixture
            .Build<ProjectRequest>()
            .With(e => e.ProjectLink, "http://example.com/project")
            .With(e => e.StartDate, DateTime.Now)
            .With(e => e.EndDate, DateTime.Now.AddDays(30))
            .Create();

        _profileRepositoryMock.Setup(repo => repo.GetProfileAsync(It.IsAny<Guid>()))
            .ReturnsAsync(projectOwner);
        _projectRepositoryMock.Setup(repo => repo.AddProjectAsync(It.IsAny<Project>()))
            .ReturnsAsync(projectRequest.ToProject());

        var actual = await _projectService.CreateProjectAsync(Guid.NewGuid(), projectRequest);
        actual.Should().Be(projectRequest.ToProject().ToProjectResponse());
    }

    [Fact]
    public async Task CreateProjectAsync_NullProjectOwner_ThrowsArgumentNullException()
    {
        var project = _fixture.Create<ProjectRequest>(); ;

        var action = () => _projectService.CreateProjectAsync(null, project);

        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task CreateProjectAsync_NullProject_ThrowsArgumentNullException()
    {
        var action = () => _projectService.CreateProjectAsync(Guid.NewGuid(), null);

        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task CreateProjectAsync_NotFoundOwner_ThrowsArgumentException()
    {
        var projectRes = _fixture.Create<ProjectRequest>();

        _profileRepositoryMock.Setup(temp => temp.GetProfileAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null as JobHunter);

        var action = () => _projectService.CreateProjectAsync(Guid.NewGuid(), projectRes);

        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task CreateProjectAsync_ProjectTitleExceedMaximum_ThrowsArgumentException()
    {
        var projectRes = _fixture.Build<ProjectRequest>()
            .With(pr => pr.ProjectTitle, new String('a', 201))
            .Create();
        var action = () => _projectService.CreateProjectAsync(Guid.NewGuid(), projectRes);

        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task CreateProjectAsync_ProjectDescriptionExceedMaximum_ThrowsArgumentException()
    {
        var projectRes = _fixture.Build<ProjectRequest>()
            .With(pr => pr.Description, new String('a', 2001))
            .Create();
        var action = () => _projectService.CreateProjectAsync(Guid.NewGuid(), projectRes);

        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task CreateProjectAsync_InvalidUrl_ThrowsArgumentException()
    {
        var projectRes = _fixture.Build<ProjectRequest>()
        .With(pr => pr.ProjectLink, "invalidprojectlink")
        .Create();

        var action = () => _projectService.CreateProjectAsync(Guid.NewGuid(), projectRes);

        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task CreateProjectAsync_LinkExceedMaximum_ThrowsArgumentException()
    {
        var projectRes = _fixture.Build<ProjectRequest>()
            .With(pr => pr.ProjectLink, "http://example.com/" + new String('a', 500))
            .Create();

        var action = () => _projectService.CreateProjectAsync(Guid.NewGuid(), projectRes);

        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task CreateProjectAsync_EndDateBeforeStartDate_ThrowsArgumentException()
    {
        var projectRes = _fixture.Build<ProjectRequest>()
            .With(pr => pr.StartDate, DateTime.Now)
            .With(pr => pr.EndDate, DateTime.Now.Subtract(TimeSpan.FromDays(1)))
            .Create();

        var action = () => _projectService.CreateProjectAsync(Guid.NewGuid(), projectRes);
        await action.Should().ThrowAsync<ArgumentException>();
    }
    #endregion

    #region DeleteProjectAsync
    [Fact]
    public async Task DeleteProjectAsync_NullProjectId_ThrowsArgumentNullException()
    {
        var action = () => _projectService.DeleteProjectAsync(null);

        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task DeleteProjectAsync_NotFoundProjectExcepton_ThrowsArgumentException()
    {
        _projectRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null as Project);

        var action = () => _projectService.DeleteProjectAsync(Guid.NewGuid());

        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task DeleteProjectAsync_ProjectDeletedSuccessfully_ReturnsDeletedProject()
    {
        var projectId = Guid.NewGuid();
        var project = new Project { ProjectId = projectId };

        _projectRepositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<Guid>()))
            .ReturnsAsync(project);

        var result = await _projectService.DeleteProjectAsync(projectId);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(project.ToProjectResponse());
    }
    #endregion

    #region DeleteMultipleProjectsAsync

    [Fact]
    public async Task DeleteMultipleProjectsAsync_NullProjectIds_ThrowsArgumentNullException()
    {
        var action = () => _projectService.DeleteMultipleProjectsAsync(null);

        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task DeleteMultipleProjectsAsync_EmptyProjectIds_ReturnsEmptyList()
    {
        var result = await _projectService.DeleteMultipleProjectsAsync([]);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task DeleteMultipleProjectsAsync_ContainsNullProjectId_IgnoreNullProjectId()
    {
        var projectIds = _fixture.CreateMany<Guid?>(5).ToList();
        projectIds[2] = null;

        _projectRepositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid? id) => new Project { ProjectId = id!.Value });

        var result = await _projectService.DeleteMultipleProjectsAsync(projectIds);

        result.Count.Should().Be(4);
    }

    [Fact]
    public async Task DeleteMultipleProjectsAsync_SomeProjectsNotFound_ReturnDeletedFoundProjects()
    {
        var projectIds = new List<Guid?> { Guid.NewGuid(), Guid.NewGuid() };

        _projectRepositoryMock.SetupSequence(repo => repo.DeleteAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null as Project)
            .ReturnsAsync(new Project() { ProjectId = projectIds[1]!.Value });

        List<ProjectResponse> res = await _projectService.DeleteMultipleProjectsAsync(projectIds);

        res.Count.Should().Be(1);
        res[0].ProjectId.Should().Be(projectIds[1]!.Value);
    }

    [Fact]
    public async Task DeleteMultipleProjectsAsync_AllProjectsDeletedSuccessfully_ReturnsTrue()
    {
        var projectIds = new List<Guid?> { Guid.NewGuid(), Guid.NewGuid() };


        _projectRepositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid? id) => new Project { ProjectId = id!.Value });

        var result = await _projectService.DeleteMultipleProjectsAsync(projectIds);

        result.Count.Should().Be(2);
    }

    #endregion

    #region GetProjectByIdAsync

    [Fact]
    public async Task GetProjectByIdAsync_NullProjectId_ThrowsArgumentNullException()
    {
        var action = () => _projectService.GetProjectByIdAsync(null);

        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task GetProjectByIdAsync_ProjectNotFound_ThrowsArgumentException()
    {
        _projectRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null as Project);

        var action = () => _projectService.GetProjectByIdAsync(Guid.NewGuid());

        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task GetProjectByIdAsync_ProjectFound_ReturnsProjectResponse()
    {
        var project = _fixture.Build<Project>()
            .With(p => p.ProjectOwner, null as JobHunter)
            .Create();
        _projectRepositoryMock.Setup(repo => repo.GetByIdAsync(project.ProjectId))
            .ReturnsAsync(project);

        var result = await _projectService.GetProjectByIdAsync(project.ProjectId);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(project.ToProjectResponse());
    }

    #endregion

    #region UpdateProjectAsync

    [Fact]
    public async Task UpdateProjectAsync_NullProjectId_ThrowsArgumentNullException()
    {
        var projectRequest = _fixture.Create<ProjectRequest>();
        var action = () => _projectService.UpdateProjectAsync(null, projectRequest);

        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpdateProjectAsync_NullProjectRequest_ThrowsArgumentNullException()
    {
        var projectId = Guid.NewGuid();
        var action = () => _projectService.UpdateProjectAsync(projectId, null);

        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpdateProjectAsync_ProjectNotFound_ThrowsArgumentException()
    {
        var projectId = Guid.NewGuid();
        var projectRequest = _fixture.Create<ProjectRequest>();

        _projectRepositoryMock.Setup(repo => repo.GetByIdAsync(projectId))
            .ReturnsAsync(null as Project);

        var action = () => _projectService.UpdateProjectAsync(projectId, projectRequest);

        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task UpdateProjectAsync_ProjectTitleExceedsMaximum_ThrowsArgumentException()
    {
        var projectId = Guid.NewGuid();
        var projectRequest = _fixture.Build<ProjectRequest>()
            .With(pr => pr.ProjectTitle, new string('a', 201))
            .Create();

        var action = () => _projectService.UpdateProjectAsync(projectId, projectRequest);

        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task UpdateProjectAsync_ProjectDescriptionExceedsMaximum_ThrowsArgumentException()
    {
        var projectId = Guid.NewGuid();
        var projectRequest = _fixture.Build<ProjectRequest>()
            .With(pr => pr.Description, new string('a', 2001))
            .Create();

        var action = () => _projectService.UpdateProjectAsync(projectId, projectRequest);

        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task UpdateProjectAsync_InvalidProjectLink_ThrowsArgumentException()
    {
        var projectId = Guid.NewGuid();
        var projectRequest = _fixture.Build<ProjectRequest>()
            .With(pr => pr.ProjectLink, "invalid-link")
            .Create();

        var action = () => _projectService.UpdateProjectAsync(projectId, projectRequest);

        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task UpdateProjectAsync_ProjectLinkExceedsMaximum_ThrowsArgumentException()
    {
        var projectId = Guid.NewGuid();
        var projectRequest = _fixture.Build<ProjectRequest>()
            .With(pr => pr.ProjectLink, "http://example.com/" + new string('a', 500))
            .Create();

        var action = () => _projectService.UpdateProjectAsync(projectId, projectRequest);

        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task UpdateProjectAsync_EndDateBeforeStartDate_ThrowsArgumentException()
    {
        var projectId = Guid.NewGuid();
        var projectRequest = _fixture.Build<ProjectRequest>()
            .With(pr => pr.StartDate, DateTime.Now)
            .With(pr => pr.EndDate, DateTime.Now.AddDays(-1))
            .Create();

        var action = () => _projectService.UpdateProjectAsync(projectId, projectRequest);

        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task UpdateProjectAsync_ValidRequest_UpdatesProjectSuccessfully()
    {
        var projectId = Guid.NewGuid();
        var projectRequest = _fixture.Build<ProjectRequest>()
            .With(pr => pr.ProjectLink, "http://example.com/project")
            .With(pr => pr.StartDate, DateTime.Now)
            .With(pr => pr.EndDate, DateTime.Now.AddDays(30))
            .Create();

        var project = projectRequest.ToProject();

        _projectRepositoryMock.Setup(repo => repo.GetByIdAsync(projectId))
            .ReturnsAsync(project);

        _projectRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Project>()))
            .ReturnsAsync((Project updatedProject) => updatedProject);

        var result = await _projectService.UpdateProjectAsync(projectId, projectRequest);

        result.Should().NotBeNull();
    }

    #endregion

    #region FilterProjectsAsync

    [Fact]
    public async Task FilterProjectsAsync_NullUserId_ThrowsArgumentNullException()
    {
        var action = () => _projectService.FilterProjectsAsync(null, "search", new List<string> { "C#" });
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task FilterProjectsAsync_NullTechnologiesOrSkillsAndNotNullKeyword_ReturnsAllProjectsForUser()
    {
        var userId = Guid.NewGuid();
        var projects = _fixture.Build<Project>()
            .With(p => p.StartDate, DateTime.Now)
            .With(p => p.EndDate, DateTime.Now.AddDays(30))
            .With(p => p.ProjectOwner, null as JobHunter)
            .CreateMany(3)
            .ToList();

        _projectRepositoryMock.Setup(
            repo => repo.GetAllProjectsWithFilterAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<List<string>>()))
            .ReturnsAsync(projects);

        var result = await _projectService.FilterProjectsAsync(userId, "keyword", null);

        result.Should().NotBeNull();
        result.Count.Should().Be(projects.Count);
    }

    [Fact]
    public async Task FilterProjectsAsync_WithSearchTermAndTechnologies_ReturnsFilteredProjects()
    {
        var userId = Guid.NewGuid();
        var searchTerm = "AI";
        var technologies = new List<string> { "Python", "ML" };
        var projects = _fixture.Build<Project>()
            .With(p => p.StartDate, DateTime.Now)
            .With(p => p.EndDate, DateTime.Now.AddDays(30))
            .With(p => p.ProjectOwner, null as JobHunter)
            .CreateMany<Project>(2).ToList();

        _projectRepositoryMock.Setup(repo => repo.GetAllProjectsWithFilterAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<List<string>>()))
            .ReturnsAsync(projects);

        var result = await _projectService.FilterProjectsAsync(userId, searchTerm, technologies);

        result.Should().NotBeNull();
        result.Count.Should().Be(projects.Count);
    }

    [Fact]
    public async Task FilterProjectsAsync_NoProjectsFound_ReturnsEmptyList()
    {
        var userId = Guid.NewGuid();
        var searchTerm = "NonExistent";
        var technologies = new List<string> { "UnknownTech" };

        _projectRepositoryMock.Setup(repo => repo.GetAllProjectsWithFilterAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<List<string>>()))
            .ReturnsAsync([]);

        var result = await _projectService.FilterProjectsAsync(userId, searchTerm, technologies);

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task FilterProjectsAsync_WithSearchTermAndNullTechOrSkills_ReturnsFilteredProjects()
    {
        var userId = Guid.NewGuid();
        var searchTerm = "AI";
        List<string>? technologies = null;
        var projects = _fixture.Build<Project>()
            .With(p => p.StartDate, DateTime.Now)
            .With(p => p.EndDate, DateTime.Now.AddDays(30))
            .With(p => p.ProjectOwner, null as JobHunter)
            .CreateMany<Project>(2).ToList();

        _projectRepositoryMock.Setup(repo => repo.GetAllProjectsWithFilterAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<List<string>>()))
            .ReturnsAsync(projects);

        var result = await _projectService.FilterProjectsAsync(userId, searchTerm, technologies);

        result.Should().NotBeNull();
        result.Count.Should().Be(projects.Count);
    }

    #endregion

    #region GetGeneralProjectInfoFromUserAsync
    [Fact]
    public async Task GetGeneralProjectInfoFromUserAsync_EmptyUserId_ShouldThrowArgumentNullException()
    {
        var actualAction = async () => await _projectService.GetGeneralProjectInfoFromUserAsync(null);
        await actualAction.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task GetGeneralProjectInfoFromUserAsync_NotFoundUserId_ShouldThrowArgumentException()
    {
        _userManagerMock.Setup(mockService => mockService.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(() => null);

        var actualAction =
            async () => await _projectService.GetGeneralProjectInfoFromUserAsync(Guid.NewGuid());
        await actualAction.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task GetGeneralProjectInfoFromUserAsync_SuccessfulGetResult_ShouldReturnResult()
    {
        _userManagerMock.Setup(mockService => mockService.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(_fixture.Create<JobHunter>());

        _projectRepositoryMock.Setup(mockRepo => mockRepo.ProjectsCountAsync(It.IsAny<Guid>()))
            .ReturnsAsync(5);

        _projectRepositoryMock.Setup(mockRepo => mockRepo.FinishedProjectsCountAsync(It.IsAny<Guid>()))
            .ReturnsAsync(3);

        _projectRepositoryMock.Setup(mockRepo => mockRepo.TechnologyUsedInProjectsCountAsync(It.IsAny<Guid>()))
            .ReturnsAsync(10);

        _projectRepositoryMock.Setup(mockRepo => mockRepo.RoleActedInProjectsCountAsync(It.IsAny<Guid>()))
            .ReturnsAsync(2);

        _projectRepositoryMock.Setup(mockRepo => mockRepo.TopFiveMostUsedTechnologyAsync(It.IsAny<Guid>()))
            .ReturnsAsync(["React", "Typescript", "ASP.NET Core"]);

        var actual = await _projectService.GetGeneralProjectInfoFromUserAsync(Guid.NewGuid());
        actual.Should().NotBeNull();
    }
    #endregion
}

