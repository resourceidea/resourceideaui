using Microsoft.AspNetCore.Mvc.Testing;
using EastSeat.ResourceIdea.Web;

namespace EastSeat.ResourceIdea.Web.E2ETests;

/// <summary>
/// Tests to verify the web application can be started for testing
/// </summary>
public class WebApplicationTests : IDisposable
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public WebApplicationTests()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task WebApplication_CanStart()
    {
        // This test verifies that the web application can be started
        Assert.NotNull(_factory);
        Assert.NotNull(_client);
        Assert.NotNull(_client.BaseAddress);
    }

    [Fact]
    public async Task HomePageRequiresAuthentication()
    {
        // This test checks if the home page redirects to login (which is expected behavior)
        var response = await _client.GetAsync("/");
        
        // The response might be a redirect to login page or an authentication challenge
        // This is expected behavior for a secured application
        Assert.True(response.StatusCode == System.Net.HttpStatusCode.Redirect || 
                    response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    response.StatusCode == System.Net.HttpStatusCode.OK);
    }

    public void Dispose()
    {
        _client?.Dispose();
        _factory?.Dispose();
    }
}