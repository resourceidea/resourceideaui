using EastSeat.ResourceIdea.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Timers;

namespace EastSeat.ResourceIdea.Web.Components.Layout;

public partial class MainLayout : IDisposable
{
    [Inject] private NotificationService NotificationService { get; set; } = default!;
    [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    private string currentUserName = "User";
    private Notification? currentNotification;
    private System.Timers.Timer? autoCloseTimer;

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        if (authState.User.Identity?.IsAuthenticated == true)
        {
            currentUserName = authState.User.Identity.Name ?? "User";
        }

        // Subscribe to notifications
        NotificationService.OnNotification += HandleNotification;
        NotificationService.OnClearNotification += ClearNotification;
        
        // Subscribe to navigation changes to clear notifications
        NavigationManager.LocationChanged += OnLocationChanged;
    }

    private void HandleNotification(Notification notification)
    {
        // Clear any existing timer
        autoCloseTimer?.Stop();
        autoCloseTimer?.Dispose();
        
        currentNotification = notification;
        StateHasChanged();

        // Set up auto-close timer if enabled
        if (notification.AutoClose)
        {
            autoCloseTimer = new System.Timers.Timer(notification.AutoCloseDelayMs);
            autoCloseTimer.Elapsed += (sender, e) => {
                InvokeAsync(() => {
                    ClearNotification();
                });
            };
            autoCloseTimer.AutoReset = false; // Only fire once
            autoCloseTimer.Start();
        }
    }

    private void OnLocationChanged(object? sender, Microsoft.AspNetCore.Components.Routing.LocationChangedEventArgs e)
    {
        // Clear notification when navigating to a different page
        if (currentNotification != null)
        {
            ClearNotification();
        }
    }

    private void ClearNotification()
    {
        // Stop and dispose timer
        autoCloseTimer?.Stop();
        autoCloseTimer?.Dispose();
        autoCloseTimer = null;
        
        currentNotification = null;
        StateHasChanged();
    }

    public void Dispose()
    {
        NotificationService.OnNotification -= HandleNotification;
        NotificationService.OnClearNotification -= ClearNotification;
        NavigationManager.LocationChanged -= OnLocationChanged;
        
        // Clean up timer
        autoCloseTimer?.Stop();
        autoCloseTimer?.Dispose();
    }
}