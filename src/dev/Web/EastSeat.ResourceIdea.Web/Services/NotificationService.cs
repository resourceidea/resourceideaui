using System;

namespace EastSeat.ResourceIdea.Web.Services;

public class NotificationMessage
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
    private NotificationMessage? _notificationMessage;

    public NotificationMessage? GetMessage()
    {
        NotificationMessage? message = _notificationMessage;
        _notificationMessage = null; // Clear the message after retrieval
        return message;
    }

    public void SetMessage(string message, NotificationType type = NotificationType.Success)
    {
        _notificationMessage = new NotificationMessage
        {
            Message = message,
            Type = type,
            Timestamp = DateTimeOffset.UtcNow
        };
    }
}
