using JobHunt.Core.DTO;
using JobHunt.Core.ServiceContracts;
using JobHunt.Ui.CustomModelBinders;
using Microsoft.AspNetCore.Mvc;

namespace JobHunt.UI.Controllers;

public class JobFilterController(
    IJobFilterService jobFilterService,
    IJobFilterByUserService jobFilterByUserService) : ApiControllerBase
{
    private readonly IJobFilterService _jobFilterService = jobFilterService;
    private readonly IJobFilterByUserService _jobFilterByUserService = jobFilterByUserService;
    [HttpGet]
    public async Task<ActionResult<JobFilterListResponse>> GetAllJobFilters(
        [ModelBinder(BinderType = typeof(UserIdBinder))] Guid? userId
    )
    {
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
        if (!userId.HasValue) return BadRequest("UserId is not defined");
        else if (userId.Value != jobFilter.UserId) return BadRequest("Invalid creation request");
        JobFilterResponseDetail jobFilterAdded =
            await _jobFilterService.CreateNewJobFilterAsync(jobFilter);
        return jobFilterAdded;
    }

    [HttpDelete("{jobFilterId}")]
    public async Task<ActionResult<JobFilterResponseSimple>> DeleteJobFilter([FromRoute] Guid? jobFilterId)
    {
        JobFilterResponseSimple jobFilterDelete = await _jobFilterService.DeleteJobFilterAsync(jobFilterId);
        return jobFilterDelete;
    }
}
