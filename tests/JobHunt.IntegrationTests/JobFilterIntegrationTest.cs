using FluentAssertions;

namespace JobHunt.IntegrationTests;

public class JobFilterIntegrationTest(CustomWebApplicationFactory factory) : 
    IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Test1()
    {
        HttpResponseMessage res = await _client.GetAsync("/jobfilter");
        res.Should().Be2XXSuccessful();
    }
}
