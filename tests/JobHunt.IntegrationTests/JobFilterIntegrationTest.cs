using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using JobHunt.Core.DTO;
using Xunit.Abstractions;

namespace JobHunt.IntegrationTests;

public class JobFilterIntegrationTest :
    IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _outputHelper;

    public JobFilterIntegrationTest(
        CustomWebApplicationFactory factory,
        ITestOutputHelper outputHelper
    )
    {
        _client = factory.CreateClient();
        _outputHelper = outputHelper;
    }

    private async Task Signup()
    {
        HttpResponseMessage res = await _client.PostAsJsonAsync("api/account/register", new
        {
            Email = "thanhkieu@gmail.com",
            Password = "Th@nh2007",
            ConfirmPassword = "Th@nh2007",
            FullName = "Kieu Tien Thanh"
        });
    }

    public async Task<AuthenticationResponse?> Signin()
    {
        HttpResponseMessage res = await _client.PostAsJsonAsync("api/account/login", new
        {
            Email = "thanhkieu@gmail.com",
            Password = "Th@nh2007"
        });
        string body = await res.Content.ReadAsStringAsync();
        return await res.Content.ReadFromJsonAsync<AuthenticationResponse>();
    }

    #region GetAllJobFilters
    [Fact]
    public async Task GetAllJobFitersAsync_GetAllJobFiltersFromUser_ShouldBeSuccessful()
    {
        await Signup();
        string jwtToken = (await this.Signin())!.Token!;
        _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(
            "Bearer " + jwtToken);
        HttpResponseMessage res = await _client.GetAsync("api/jobfilter/");
        res.Should().Be2XXSuccessful();
    }
    #endregion
}
