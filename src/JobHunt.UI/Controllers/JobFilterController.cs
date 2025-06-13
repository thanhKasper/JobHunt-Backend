using JobHunt.Core.DTO;
using JobHunt.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace JobHunt.UI.Controllers;

public class JobFilterController(IJobFilterService jobFilterService) : ApiControllerBase
{
    private readonly IJobFilterService _jobFilterService = jobFilterService;
    [HttpGet]
    public async Task<ActionResult<List<JobFilterResponseSimple>>> Get()
    {
        List<JobFilterResponseSimple> jobFilterList = await _jobFilterService.GetAllJobFilterSimple();
        return jobFilterList;
    }

    [HttpGet("{jobfilter_id}")]
    public async Task<ActionResult<JobFilterResponseDetail>> GetOneJobFilter([FromRoute(Name = "jobfilter_id")] Guid jobfilterId)
    {
        JobFilterResponseDetail jobFilterDetail = await _jobFilterService.GetJobFilterDetail(jobfilterId);
        return jobFilterDetail;
    }

    // [HttpPut]
    // public async Task<IActionResult> UpdateJobFilter()
    // {
    //     return Ok("Receive request from PUT api/jobfilter");
    // }

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
