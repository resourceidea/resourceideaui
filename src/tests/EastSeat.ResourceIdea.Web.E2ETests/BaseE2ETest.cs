using Microsoft.Playwright;
using Microsoft.AspNetCore.Mvc.Testing;
using EastSeat.ResourceIdea.Web;
using Xunit;

namespace EastSeat.ResourceIdea.Web.E2ETests;

/// <summary>
/// Base test class providing common setup for E2E tests
/// </summary>
public class BaseE2ETest : IDisposable
{
    protected readonly WebApplicationFactory<Program> _factory;
    protected readonly HttpClient _httpClient;
    protected readonly string _baseUrl;
    protected IPlaywright? _playwright;
    protected IBrowser? _browser;
    protected IPage? _page;

    public BaseE2ETest()
    {
        _factory = new WebApplicationFactory<Program>();
        _httpClient = _factory.CreateClient();
        _baseUrl = _httpClient.BaseAddress?.ToString() ?? "https://localhost";
    }

    protected async Task InitializePlaywrightAsync()
    {
        try
        {
            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true
            });
            _page = await _browser.NewPageAsync();
        }
        catch (PlaywrightException ex) when (ex.Message.Contains("Executable doesn't exist"))
        {
            // Browser not installed - re-throw to let test handle this
            throw;
        }
    }

    protected async Task NavigateToAsync(string path)
    {
        if (_page == null)
            await InitializePlaywrightAsync();
            
        var url = $"{_baseUrl}{path}";
        await _page!.GotoAsync(url);
    }

    public void Dispose()
    {
        _page?.CloseAsync().Wait();
        _browser?.CloseAsync().Wait();
        _playwright?.Dispose();
        _httpClient?.Dispose();
        _factory?.Dispose();
    }
}
