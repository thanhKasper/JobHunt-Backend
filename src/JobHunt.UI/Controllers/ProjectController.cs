using JobHunt.Core.DTO;
using JobHunt.Core.ServiceContracts;
using JobHunt.Ui.CustomModelBinders;
using Microsoft.AspNetCore.Mvc;

namespace JobHunt.UI.Controllers;

public class ProjectController : ApiControllerBase
{
    private readonly ILogger<ProjectController> _logger;
    private readonly IProjectService _projectService;

    public ProjectController(ILogger<ProjectController> logger, IProjectService projectService)
    {
        _projectService = projectService;
        _logger = logger;
    }

    [HttpGet("{projectId}")]
    public async Task<ActionResult<ProjectResponse>> GetProject(Guid projectId)
    {
        var project = await _projectService.GetProjectByIdAsync(projectId);

        return project;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProjectResponse>>> GetAllWithFilter(
        [ModelBinder(BinderType = typeof(UserIdBinder))] Guid userId,
        [FromQuery] string? searchTerm,
        [FromQuery] List<string>? technologiesOrSkills)
    {
        var projects = await _projectService.FilterProjectsAsync(userId, searchTerm, technologiesOrSkills);
        return projects;
    }

    [HttpGet("general-info")]
    public async Task<ActionResult<ProjectGeneralInfoResponse>> GetUserGeneralProjectsInformation(
        [ModelBinder(BinderType = typeof(UserIdBinder))] Guid? userId)
    {
        return await _projectService.GetGeneralProjectInfoFromUserAsync(userId);
    }

    [HttpPost]
    public async Task<ActionResult<ProjectResponse>> Create(
        [ModelBinder(BinderType = typeof(UserIdBinder))] Guid? userId,
        [FromBody] ProjectRequest project
    )
    {
        var newProject = await _projectService.CreateProjectAsync(userId, project);
        return newProject;
    }

    [HttpPut("{projectId}")]
    public async Task<ActionResult<ProjectResponse>> Update(Guid projectId, [FromBody] ProjectRequest projectRequest)
    {
        var updatedProject = await _projectService.UpdateProjectAsync(projectId, projectRequest);
        return updatedProject;
    }

    [HttpDelete]
    public async Task<ActionResult<IEnumerable<ProjectResponse>>> Delete([FromBody] List<Guid?>? projectIds)
    {
        var deletedProjects = await _projectService.DeleteMultipleProjectsAsync(projectIds);
        return deletedProjects;
    }

    [HttpDelete("{projectId}")]
    public async Task<ActionResult<ProjectResponse>> DeleteOneProject(
        Guid? projectId)
    {
        var deletedProject = await _projectService.DeleteProjectAsync(projectId);
        return deletedProject;
    }
}