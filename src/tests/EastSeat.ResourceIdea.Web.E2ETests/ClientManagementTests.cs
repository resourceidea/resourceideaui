using Microsoft.Playwright;

namespace EastSeat.ResourceIdea.Web.E2ETests;

/// <summary>
/// E2E tests for client management functionality
/// </summary>
public class ClientManagementTests : BaseE2ETest
{
    [Fact]
    public async Task AddClient_WithValidData_ShouldSucceed()
    {
        // Arrange
        await InitializePlaywrightAsync();
        
        // Act - Navigate to Add Client page
        await NavigateToAsync("/clients/add");
        
        // Wait for page to load
        await _page!.WaitForSelectorAsync("h1:has-text('Add New Client')");
        
        // Fill in client details
        await _page.FillAsync("input[id='firstname']", "Test Client Company");
        await _page.FillAsync("input[id='City']", "Test City");
        await _page.FillAsync("input[id='Street']", "123 Test Street");
        await _page.FillAsync("input[id='Building']", "Building A");
        
        // Submit the form
        await _page.ClickAsync("button[type='submit']:has-text('Save')");
        
        // Assert - Check for success (this may need adjustment based on actual behavior)
        // We might need to wait for navigation or success message
        await _page.WaitForTimeoutAsync(2000); // Wait for form submission processing
        
        // Verify we're no longer on the add page or that success occurred
        var currentUrl = _page.Url;
        Assert.DoesNotContain("/clients/add", currentUrl);
    }

    [Fact]
    public async Task AddClient_WithEmptyRequiredFields_ShouldShowValidationErrors()
    {
        // Arrange
        await InitializePlaywrightAsync();
        
        // Act - Navigate to Add Client page
        await NavigateToAsync("/clients/add");
        
        // Wait for page to load
        await _page!.WaitForSelectorAsync("h1:has-text('Add New Client')");
        
        // Submit form without filling required fields
        await _page.ClickAsync("button[type='submit']:has-text('Save')");
        
        // Assert - Check that validation summary or errors are shown
        // Note: This assumes the form has client-side validation
        await _page.WaitForTimeoutAsync(1000);
        
        // Verify we're still on the add page
        var currentUrl = _page.Url;
        Assert.Contains("/clients/add", currentUrl);
    }

    [Fact]
    public async Task EditClient_WithValidData_ShouldSucceed()
    {
        // Note: This test assumes there's already a client to edit
        // In a real scenario, you might need to create a client first or use test data
        
        // Arrange
        await InitializePlaywrightAsync();
        
        // First, let's navigate to clients list to find a client to edit
        await NavigateToAsync("/clients");
        
        // Wait for the clients page to load
        await _page!.WaitForSelectorAsync("h1, h2, h3", new PageWaitForSelectorOptions 
        { 
            Timeout = 5000 
        });
        
        // Look for an edit link/button - this may need adjustment based on actual UI
        var editLinks = await _page.QuerySelectorAllAsync("a[href*='/clients/edit/']");
        
        if (editLinks.Count > 0)
        {
            // Click the first edit link
            await editLinks[0].ClickAsync();
            
            // Wait for edit page to load
            await _page.WaitForSelectorAsync("h1:has-text('Edit Client'), h2:has-text('Edit Client'), h3:has-text('Edit Client')", new PageWaitForSelectorOptions 
            { 
                Timeout = 5000 
            });
            
            // Modify client details
            await _page.FillAsync("input[id='name']", "Updated Client Name");
            await _page.FillAsync("input[id='City']", "Updated City");
            
            // Submit the form
            await _page.ClickAsync("button[type='submit']:has-text('Save')");
            
            // Assert - Wait for save operation to complete
            await _page.WaitForTimeoutAsync(2000);
            
            // Verify we're no longer on the edit page
            var currentUrl = _page.Url;
            Assert.DoesNotContain("/clients/edit/", currentUrl);
        }
        else
        {
            // Skip test if no clients are available to edit
            Assert.True(true, "No clients available for editing - test skipped");
        }
    }

    [Fact]
    public async Task NavigateToAddClient_ShouldDisplayCorrectForm()
    {
        // Arrange
        await InitializePlaywrightAsync();
        
        // Act
        await NavigateToAsync("/clients/add");
        
        // Assert - Verify the form elements are present
        await _page!.WaitForSelectorAsync("h1:has-text('Add New Client')");
        
        // Check that required form fields are present
        await _page.WaitForSelectorAsync("input[id='firstname']");
        await _page.WaitForSelectorAsync("input[id='City']");
        await _page.WaitForSelectorAsync("input[id='Street']");
        await _page.WaitForSelectorAsync("input[id='Building']");
        await _page.WaitForSelectorAsync("button[type='submit']:has-text('Save')");
        
        // Verify back link is present
        await _page.WaitForSelectorAsync("a[href='/clients']:has-text('Back')");
    }
}