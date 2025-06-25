using Microsoft.AspNetCore.Components;

namespace EastSeat.ResourceIdea.Web.UnitTests.TestHelpers;

/// <summary>
/// Test implementation of NavigationManager that tracks navigation calls.
/// </summary>
public class TestNavigationManager : NavigationManager
{
    public TestNavigationManager() : base()
    {
        Initialize("https://localhost/", "https://localhost/");
    }

    public TestNavigationManager(string baseUri, string uri) : base()
    {
        Initialize(baseUri, uri);
    }

    /// <summary>
    /// List of all navigation calls made during testing.
    /// </summary>
    public List<NavigationCall> NavigationCalls { get; } = new();

    /// <summary>
    /// Gets the last navigation call made.
    /// </summary>
    public NavigationCall? LastNavigationCall => NavigationCalls.LastOrDefault();

    /// <summary>
    /// Gets the count of navigation calls made.
    /// </summary>
    public int NavigationCallCount => NavigationCalls.Count;

    protected override void NavigateToCore(string uri, bool forceLoad)
    {
        NavigationCalls.Add(new NavigationCall(uri, forceLoad));
    }

    protected override void NavigateToCore(string uri, NavigationOptions options)
    {
        NavigationCalls.Add(new NavigationCall(uri, options.ForceLoad, options.ReplaceHistoryEntry));
    }

    /// <summary>
    /// Verifies that a specific navigation call was made.
    /// </summary>
    /// <param name="expectedUri">The expected URI that should have been navigated to.</param>
    /// <param name="expectedForceLoad">The expected forceLoad value.</param>
    /// <returns>True if the navigation call was found, false otherwise.</returns>
    public bool WasNavigatedTo(string expectedUri, bool expectedForceLoad = false)
    {
        return NavigationCalls.Any(call => call.Uri == expectedUri && call.ForceLoad == expectedForceLoad);
    }

    /// <summary>
    /// Verifies that exactly one navigation call was made with the specified parameters.
    /// </summary>
    /// <param name="expectedUri">The expected URI that should have been navigated to.</param>
    /// <param name="expectedForceLoad">The expected forceLoad value.</param>
    public void VerifyNavigatedTo(string expectedUri, bool expectedForceLoad = false)
    {
        var matchingCalls = NavigationCalls.Where(call => call.Uri == expectedUri && call.ForceLoad == expectedForceLoad).ToList();

        if (matchingCalls.Count == 0)
        {
            throw new InvalidOperationException($"Expected navigation to '{expectedUri}' with forceLoad={expectedForceLoad}, but no matching navigation call was found. Actual calls: {string.Join(", ", NavigationCalls)}");
        }

        if (matchingCalls.Count > 1)
        {
            throw new InvalidOperationException($"Expected exactly one navigation to '{expectedUri}' with forceLoad={expectedForceLoad}, but found {matchingCalls.Count} calls.");
        }
    }

    /// <summary>
    /// Verifies that exactly the expected number of navigation calls were made.
    /// </summary>
    /// <param name="expectedCount">The expected number of navigation calls.</param>
    public void VerifyNavigationCallCount(int expectedCount)
    {
        if (NavigationCallCount != expectedCount)
        {
            throw new InvalidOperationException($"Expected {expectedCount} navigation calls, but found {NavigationCallCount}. Actual calls: {string.Join(", ", NavigationCalls)}");
        }
    }

    /// <summary>
    /// Clears all recorded navigation calls.
    /// </summary>
    public void ClearNavigationCalls()
    {
        NavigationCalls.Clear();
    }
}

/// <summary>
/// Represents a navigation call made during testing.
/// </summary>
public record NavigationCall(string Uri, bool ForceLoad, bool ReplaceHistoryEntry = false)
{
    public override string ToString() => $"NavigateTo('{Uri}', forceLoad: {ForceLoad}, replaceHistoryEntry: {ReplaceHistoryEntry})";
}
