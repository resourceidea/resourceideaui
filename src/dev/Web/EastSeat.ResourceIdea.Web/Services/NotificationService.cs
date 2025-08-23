namespace EastSeat.ResourceIdea.Web.Services;

public class Notification
{
    public string Message { get; set; } = string.Empty;
    public NotificationType Type { get; set; } = NotificationType.Info;
    public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;
    public bool AutoClose { get; set; } = true;
    public int AutoCloseDelayMs { get; set; } = 3000; // 3 seconds
}

public enum NotificationType
{
    Info,
    Success,
    Warning,
    Error
}

public class NotificationService
{
    public event Action<Notification>? OnNotification;
    public event Action? OnClearNotification;

    public void ShowNotification(Notification notification)
    {
        OnNotification?.Invoke(notification);
    }

    public void ShowSuccessNotification(string message, bool autoClose = true)
    {
        ShowNotification(new Notification { Type = NotificationType.Success, Message = message, AutoClose = autoClose });
    }

    public void ShowErrorNotification(string message, bool autoClose = true)
    {
        ShowNotification(new Notification { Type = NotificationType.Error, Message = message, AutoClose = autoClose });
    }

    public void ShowWarningNotification(string message, bool autoClose = true)
    {
        ShowNotification(new Notification { Type = NotificationType.Warning, Message = message, AutoClose = autoClose });
    }

    public void ShowInfoNotification(string message, bool autoClose = true)
    {
        ShowNotification(new Notification { Type = NotificationType.Info, Message = message, AutoClose = autoClose });
    }

    public void ClearNotification()
    {
        OnClearNotification?.Invoke();
    }
}
