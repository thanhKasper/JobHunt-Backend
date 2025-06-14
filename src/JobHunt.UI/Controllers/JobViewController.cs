using JobHunt.Core.DTO;
using JobHunt.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace JobHunt.UI.Controllers;

public class JobViewController(IJobViewService jobViewService, ILogger<JobViewController> logger) : ApiControllerBase
{
    private readonly IJobViewService _jobViewService = jobViewService;
    private readonly ILogger<JobViewController> _logger = logger;

    [HttpGet]
    public async Task<ActionResult<List<JobResponse>>> GetAllJobs()
    {
        _logger.LogInformation("Calling GetAllJobs method");
        var jobs = await _jobViewService.GetAllMatchingJobs();
        return jobs;
    }

    [HttpGet("{jobFilterId}")]
    public async Task<ActionResult<List<JobResponse>>> GetAllJobsByFilterId(Guid jobFilterId)
    {
        _logger.LogInformation("Calling GetAllJobsByFilterId method");
        var jobs = await _jobViewService.GetAllMatchingJobsBaseOnJobFilter(jobFilterId);
        return jobs;
    }
}