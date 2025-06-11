using JobHunt.Core.DTO;
using Microsoft.AspNetCore.Mvc;

namespace JobHunt.UI.Controllers;

public class JobFilterController : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok("Receive Request from GET api/jobfilter/");
    }

    [HttpGet("{jobfilter_id}")]
    public async Task<IActionResult> GetOneJobFilter([FromRoute(Name = "jobfilter_id")]Guid jobfilterId)
    {
        return Ok($"Return get request from job filter id: {jobfilterId}");
    }

    [HttpPut]
    public async Task<IActionResult> UpdateJobFilter()
    {
        return Ok("Receive request from PUT api/jobfilter");
    }

    [HttpPost]
    public async Task<ActionResult<JobFilterDTO>> CreateNewJobFilter([FromBody] JobFilterDTO jobFilter)
    {
        return jobFilter;
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteJobFilter()
    {
        return Ok("Receive deletel job filter request");
    }
}
