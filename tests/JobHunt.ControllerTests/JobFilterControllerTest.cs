using JobHunt.Core.ServiceContracts;
using JobHunt.Core.Services;
using JobHunt.UI.Controllers;
using Moq;

namespace JobHunt.ControllerTests;

public class JobFilterControllerTest
{
    private readonly IJobFilterService _jobFilterService;
    private readonly IJobFilterByUserService _jobFilterByUserService;

    public JobFilterControllerTest()
    {
        _jobFilterService = new Mock<JobFilterService>().Object;
        _jobFilterByUserService = new Mock<JobFilterByUserService>().Object;
    }


    [Fact]
    public void Test1()
    {

    }
}
