using Microsoft.AspNetCore.Components;
using EastSeat.ResourceIdea.Web.Components.Base;
using System.Net.Http.Json;

namespace EastSeat.ResourceIdea.Web.Components.Pages.Account;

/// <summary>
/// Logout page component that handles user sign out.
/// </summary>
public partial class Logout : ResourceIdeaComponentBase
{
    [Inject] private HttpClient HttpClient { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;

    /// <summary>
    /// Component initialization - automatically signs out the user.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        await ExecuteAsync(async () =>
        {
            var response = await HttpClient.PostAsync("/api/account/logout", null);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LogoutResponse>();
                if (result?.Success == true)
                {
                    // Redirect to home page after logout
                    Navigation.NavigateTo(result.RedirectUrl ?? "/", forceLoad: true);
                }
                else
                {
                    // Even if API call fails, redirect to home page
                    Navigation.NavigateTo("/", forceLoad: true);
                }
            }
            else
            {
                // Even if logout fails, redirect to home page
                Navigation.NavigateTo("/", forceLoad: true);
            }
        }, "User logout");
    }

    /// <summary>
    /// Response model for logout API calls.
    /// </summary>
    public class LogoutResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? RedirectUrl { get; set; }
    }
}
