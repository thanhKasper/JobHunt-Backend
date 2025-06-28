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
        var jobFilterList =
            await _jobFilterByUserService.GetAllJobFiltersFromUser(userId);
        return jobFilterList;
    }

    [HttpGet("{jobfilter_id}")]
    public async Task<ActionResult<JobFilterResponseDetail>> GetOneJobFilter([FromRoute(Name = "jobfilter_id")] Guid jobfilterId)
    {
        JobFilterResponseDetail jobFilterDetail = await _jobFilterService.GetJobFilterDetail(jobfilterId);
        return jobFilterDetail;
    }

    [HttpPost]
    public async Task<ActionResult<JobFilterResponseDetail>> CreateNewJobFilter([FromBody] JobFilterCreationRequest jobFilter)
    {
        JobFilterResponseDetail jobFilterAdded = await _jobFilterService.CreateNewJobFilter(jobFilter);
        return jobFilterAdded;
    }

    [HttpDelete("{jobFilterId}")]
    public async Task<ActionResult<JobFilterResponseSimple>> DeleteJobFilter([FromRoute] Guid? jobFilterId)
    {
        JobFilterResponseSimple jobFilterDelete = await _jobFilterService.DeleteJobFilter(jobFilterId);
        return jobFilterDelete;
    }
}
