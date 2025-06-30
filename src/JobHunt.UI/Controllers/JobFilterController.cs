using JobHunt.Core.DTO;
using JobHunt.Core.ServiceContracts;
using JobHunt.Ui.CustomModelBinders;
using Microsoft.AspNetCore.Mvc;

namespace JobHunt.UI.Controllers;

public class JobFilterController(
    IJobFilterService jobFilterService,
    IJobFilterByUserService jobFilterByUserService,
    ILogger<JobFilterController> logger) : ApiControllerBase
{
    private readonly IJobFilterService _jobFilterService = jobFilterService;
    private readonly IJobFilterByUserService _jobFilterByUserService = jobFilterByUserService;
    private readonly ILogger<JobFilterController> _logger = logger;
    [HttpGet]
    public async Task<ActionResult<JobFilterListResponse>> GetAllJobFilters(
        [ModelBinder(BinderType = typeof(UserIdBinder))] Guid? userId
    )
    {
        _logger.LogInformation("JOBHUNT - Calling GetAllJobFilters method");
        if (!userId.HasValue) return BadRequest("UserId is not provided");
        var jobFilterList =
            await _jobFilterByUserService.GetAllJobFiltersFromUserAsync(userId);
        return jobFilterList;
    }

    [HttpGet("{jobfilter_id}")]
    public async Task<ActionResult<JobFilterResponseDetail>> GetOneJobFilter([FromRoute(Name = "jobfilter_id")] Guid jobfilterId)
    {
        JobFilterResponseDetail jobFilterDetail = await _jobFilterService.GetJobFilterDetailAsync(jobfilterId);
        return jobFilterDetail;
    }

    [HttpPost]
    public async Task<ActionResult<JobFilterResponseDetail>> CreateNewJobFilter(
        [ModelBinder(BinderType = typeof(UserIdBinder))] Guid? userId,
        [FromBody] JobFilterCreationRequest jobFilter)
    {
        _logger.LogInformation("JOBHUNT - Create New Job Filter");

        if (!userId.HasValue) return BadRequest("UserId is not defined");

        JobFilterResponseDetail jobFilterAdded =
            await _jobFilterService.CreateNewJobFilterAsync(jobFilter, userId);
        return jobFilterAdded;
    }

    [HttpDelete("{jobFilterId}")]
    public async Task<ActionResult<JobFilterResponseSimple>> DeleteJobFilter([FromRoute] Guid? jobFilterId)
    {
        JobFilterResponseSimple jobFilterDelete = await _jobFilterService.DeleteJobFilterAsync(jobFilterId);
        return jobFilterDelete;
    }

    [HttpPut("active/{jobfilterId}")]
    public async Task<ActionResult<bool>> ToggleJobFilterActiveState(Guid? jobFilterId)
    {
        if (!jobFilterId.HasValue) return BadRequest("Empty job filter id");
        bool result = await _jobFilterService.ToggleJobFilterActiveStateAsync(jobFilterId);
        return result;
    }

    [HttpPut("star/{jobfilterId}")]
    public async Task<ActionResult<bool>> ToggleJobFilterStar(Guid? jobFilterId)
    {
        if (!jobFilterId.HasValue) return BadRequest("Empty job filter id");
        bool result = await _jobFilterService.ToggleJobFilterStarStateAsync(jobFilterId);
        return result;
    }
}
