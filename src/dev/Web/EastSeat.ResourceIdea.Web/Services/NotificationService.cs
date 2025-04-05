namespace EastSeat.ResourceIdea.Web.Services;

public class Notification
{
    public string Message { get; set; } = string.Empty;
    public NotificationType Type { get; set; } = NotificationType.Info;
    public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;
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

    public void ShowSuccessNotification(string message)
    {
        ShowNotification(new Notification { Type = NotificationType.Success, Message = message });
    }

    public void ShowErrorNotification(string message)
    {
        ShowNotification(new Notification { Type = NotificationType.Error, Message = message });
    }

    public void ShowWarningNotification(string message)
    {
        ShowNotification(new Notification { Type = NotificationType.Warning, Message = message });
    }

    public void ShowInfoNotification(string message)
    {
        ShowNotification(new Notification { Type = NotificationType.Info, Message = message });
    }

    public void ClearNotification()
    {
        OnClearNotification?.Invoke();
    }
}
