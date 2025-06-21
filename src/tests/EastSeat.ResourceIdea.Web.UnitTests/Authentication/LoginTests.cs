using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using EastSeat.ResourceIdea.DataStore.Identity.Entities;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace EastSeat.ResourceIdea.Web.UnitTests.Authentication;

public class LoginTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public LoginTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task LoginPage_Should_Be_Accessible_Without_Authentication()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/login");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task HomePage_Should_Be_Accessible_Without_Authentication()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task ProtectedPage_Should_Redirect_To_Login_When_Unauthenticated()
    {
        // Arrange
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        // Act
        var response = await client.GetAsync("/departments");

        // Assert
        Assert.True(response.StatusCode == HttpStatusCode.Redirect || 
                   response.StatusCode == HttpStatusCode.Found);
        
        var location = response.Headers.Location?.ToString();
        Assert.Contains("/login", location);
    }
}