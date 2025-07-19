using Microsoft.Playwright;

namespace EastSeat.ResourceIdea.Web.E2ETests.Helpers;

/// <summary>
/// Helper class for common E2E test operations
/// </summary>
public static class TestHelpers
{
    public static class Client
    {
        public const string DefaultName = "E2E Test Client";
        public const string DefaultCity = "Test City";
        public const string DefaultStreet = "123 Test Street";
        public const string DefaultBuilding = "Building A";
        
        public const string UpdatedName = "Updated E2E Test Client";
        public const string UpdatedCity = "Updated Test City";
    }
    
    public static class Engagement
    {
        public const string DefaultTitle = "E2E Test Engagement";
        public const string DefaultDescription = "This is a test engagement for E2E testing purposes.";
        public const string DefaultStatus = "InProgress";
        
        public const string UpdatedDescription = "Updated engagement description for E2E test";
        public const string UpdatedStatus = "Completed";
    }
    
    public static class Selectors
    {
        // Page headers
        public const string AddClientHeader = "h1:has-text('Add New Client')";
        public const string EditClientHeader = "h1:has-text('Edit Client'), h2:has-text('Edit Client'), h3:has-text('Edit Client')";
        public const string AddEngagementHeader = "h1:has-text('Add New Engagement')";
        public const string EditEngagementHeader = "h1:has-text('Edit Engagement'), h2:has-text('Edit Engagement'), h3:has-text('Edit Engagement')";
        
        // Form fields
        public const string ClientNameField = "input[id='firstname']";
        public const string ClientEditNameField = "input[id='name']";
        public const string ClientCityField = "input[id='City']";
        public const string ClientStreetField = "input[id='Street']";
        public const string ClientBuildingField = "input[id='Building']";
        
        public const string EngagementClientSelect = "select[id='client']";
        public const string EngagementTitleField = "input[id='title']";
        public const string EngagementDescriptionField = "textarea[id='description']";
        public const string EngagementDueDateField = "input[id='dueDate']";
        public const string EngagementStatusSelect = "select[id='status']";
        
        // Buttons
        public const string SaveButton = "button[type='submit']:has-text('Save')";
        public const string BackButton = "a:has-text('Back')";
        
        // Links
        public const string EditClientLink = "a[href*='/clients/edit/']";
        public const string EditEngagementLink = "a[href*='/engagements/edit/']";
    }
    
    public static class Timeouts
    {
        public const int DefaultWait = 5000;
        public const int FormSubmission = 2000;
        public const int PageLoad = 5000;
        public const int ShortWait = 1000;
    }
    
    /// <summary>
    /// Fills client form fields with default or specified values
    /// </summary>
    public static async Task FillClientFormAsync(IPage page, 
        string? name = null, 
        string? city = null, 
        string? street = null, 
        string? building = null,
        bool isEditForm = false)
    {
        var nameSelector = isEditForm ? Selectors.ClientEditNameField : Selectors.ClientNameField;
        
        await page.FillAsync(nameSelector, name ?? Client.DefaultName);
        await page.FillAsync(Selectors.ClientCityField, city ?? Client.DefaultCity);
        await page.FillAsync(Selectors.ClientStreetField, street ?? Client.DefaultStreet);
        await page.FillAsync(Selectors.ClientBuildingField, building ?? Client.DefaultBuilding);
    }
    
    /// <summary>
    /// Fills engagement form fields with default or specified values
    /// </summary>
    public static async Task FillEngagementFormAsync(IPage page,
        string? clientValue = null,
        string? title = null,
        string? description = null,
        string? dueDate = null,
        string? status = null)
    {
        // Select client if provided
        if (!string.IsNullOrEmpty(clientValue))
        {
            await page.SelectOptionAsync(Selectors.EngagementClientSelect, clientValue);
        }
        
        await page.FillAsync(Selectors.EngagementTitleField, title ?? Engagement.DefaultTitle);
        await page.FillAsync(Selectors.EngagementDescriptionField, description ?? Engagement.DefaultDescription);
        
        if (!string.IsNullOrEmpty(dueDate))
        {
            await page.FillAsync(Selectors.EngagementDueDateField, dueDate);
        }
        
        if (!string.IsNullOrEmpty(status))
        {
            await page.SelectOptionAsync(Selectors.EngagementStatusSelect, status);
        }
        else
        {
            await page.SelectOptionAsync(Selectors.EngagementStatusSelect, Engagement.DefaultStatus);
        }
    }
    
    /// <summary>
    /// Gets the first available client option from the engagement form
    /// </summary>
    public static async Task<string?> GetFirstAvailableClientAsync(IPage page)
    {
        var clientOptions = await page.QuerySelectorAllAsync("select[id='client'] option[value!='']");
        if (clientOptions.Count > 0)
        {
            return await clientOptions[0].GetAttributeAsync("value");
        }
        return null;
    }
    
    /// <summary>
    /// Waits for form submission to complete and verifies navigation
    /// </summary>
    public static async Task WaitForFormSubmissionAsync(IPage page, string expectedUrlExclusion)
    {
        await page.WaitForTimeoutAsync(Timeouts.FormSubmission);
        var currentUrl = page.Url;
        Assert.DoesNotContain(expectedUrlExclusion, currentUrl);
    }
    
    /// <summary>
    /// Verifies that required form elements are present on the page
    /// </summary>
    public static async Task VerifyFormElementsAsync(IPage page, params string[] selectors)
    {
        foreach (var selector in selectors)
        {
            await page.WaitForSelectorAsync(selector, new PageWaitForSelectorOptions { Timeout = Timeouts.DefaultWait });
        }
    }
}