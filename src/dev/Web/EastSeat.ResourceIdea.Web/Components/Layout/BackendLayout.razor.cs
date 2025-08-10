using EastSeat.ResourceIdea.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace EastSeat.ResourceIdea.Web.Components.Layout;

public partial class BackendLayout : IDisposable
{
    [Inject] private NotificationService NotificationService { get; set; } = default!;
    [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

    private string currentUserName = "Backend User";
    private string currentUserRole = "Unknown";
    private Notification? currentNotification;

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        if (authState.User.Identity?.IsAuthenticated == true)
        {
            currentUserName = authState.User.Identity.Name ?? "Backend User";
            
            // Determine user role
            if (authState.User.IsInRole("Developer"))
            {
                currentUserRole = "Developer";
            }
            else if (authState.User.IsInRole("Support"))
            {
                currentUserRole = "Support";
            }
            else
            {
                currentUserRole = "Backend";
            }
        }

        // Subscribe to notifications
        NotificationService.OnNotification += HandleNotification;
        NotificationService.OnClearNotification += ClearNotification;
    }

    private void HandleNotification(Notification notification)
    {
        currentNotification = notification;
        StateHasChanged();
    }

    private void ClearNotification()
    {
        currentNotification = null;
        StateHasChanged();
    }

    public void Dispose()
    {
        NotificationService.OnNotification -= HandleNotification;
        NotificationService.OnClearNotification -= ClearNotification;
    }
}