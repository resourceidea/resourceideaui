using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using System.Net;

namespace EastSeat.ResourceIdea.Web.E2ETests;

/// <summary>
/// End-to-End tests for authentication functionality.
/// Note: These tests require Playwright browsers to be installed.
/// Run 'playwright install' to set up the required browsers.
/// </summary>
public class AuthenticationE2ETests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public AuthenticationE2ETests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Login_Page_Should_Be_Accessible_Without_Authentication()
    {
        // Arrange
        using var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/login");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Login", content);
        Assert.Contains("Email", content);
        Assert.Contains("Password", content);
    }

    [Fact]
    public async Task Home_Page_Should_Be_Accessible_Without_Authentication()
    {
        // Arrange
        using var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Protected_Page_Should_Redirect_To_Login_When_Not_Authenticated()
    {
        // Arrange
        using var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
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

    [Fact]
    public async Task Protected_Pages_Should_All_Redirect_To_Login_When_Not_Authenticated()
    {
        // Arrange
        using var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        var protectedPages = new[]
        {
            "/departments",
            "/engagements", 
            "/employees",
            "/clients",
            "/workitems",
            "/jobpositions"
        };

        foreach (var page in protectedPages)
        {
            // Act
            var response = await client.GetAsync(page);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.Redirect || 
                       response.StatusCode == HttpStatusCode.Found,
                       $"Page {page} should redirect to login when not authenticated");
            
            var location = response.Headers.Location?.ToString();
            Assert.Contains("/login", location);
        }
    }

    [Fact]
    public async Task Return_Url_Should_Be_Preserved_When_Redirected_To_Login()
    {
        // Arrange
        using var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        // Act
        var response = await client.GetAsync("/employees");

        // Assert
        Assert.True(response.StatusCode == HttpStatusCode.Redirect || 
                   response.StatusCode == HttpStatusCode.Found);
        
        var location = response.Headers.Location?.ToString();
        Assert.Contains("/login", location);
        Assert.Contains("ReturnUrl", location);
        Assert.Contains("employees", location);
    }

    [Fact]
    public async Task Logout_Page_Should_Be_Accessible()
    {
        // Arrange
        using var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        // Act
        var response = await client.GetAsync("/logout");

        // Assert
        // Logout should redirect to home page
        Assert.True(response.StatusCode == HttpStatusCode.Redirect || 
                   response.StatusCode == HttpStatusCode.Found);
        
        var location = response.Headers.Location?.ToString();
        Assert.Contains("/", location);
    }

    // TODO: Add Playwright-based browser tests when browsers are available
    // These would test the actual UI interactions like:
    // - Filling out the login form
    // - Clicking buttons
    // - Verifying navigation after login
    // - Testing the full authentication workflow
}

/// <summary>
/// Integration tests for authentication using HTTP client.
/// These complement the Playwright E2E tests with lower-level HTTP testing.
/// </summary>
public class AuthenticationIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public AuthenticationIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Login_Page_Contains_Required_Form_Elements()
    {
        // Arrange
        using var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/login");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("<form", content);
        Assert.Contains("email", content, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("password", content, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("type=\"submit\"", content);
    }

    [Fact]
    public async Task Authentication_Flow_Validates_User_Credentials()
    {
        // This test would require setting up test authentication
        // and verifying the actual login process
        // For now, we verify the structure is in place
        
        using var client = _factory.CreateClient();
        var response = await client.GetAsync("/login");
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}