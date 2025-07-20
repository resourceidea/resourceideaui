using EastSeat.ResourceIdea.Web.Services;
using Xunit;

namespace EastSeat.ResourceIdea.Web.UnitTests.Services;

public class NotificationServiceTests
{
    [Fact]
    public void ShowSuccessNotification_ShouldTriggerOnNotificationEvent()
    {
        // Arrange
        var notificationService = new NotificationService();
        Notification? receivedNotification = null;
        notificationService.OnNotification += (notification) => receivedNotification = notification;

        // Act
        notificationService.ShowSuccessNotification("Test success message");

        // Assert
        Assert.NotNull(receivedNotification);
        Assert.Equal(NotificationType.Success, receivedNotification.Type);
        Assert.Equal("Test success message", receivedNotification.Message);
    }

    [Fact]
    public void ShowErrorNotification_ShouldTriggerOnNotificationEvent()
    {
        // Arrange
        var notificationService = new NotificationService();
        Notification? receivedNotification = null;
        notificationService.OnNotification += (notification) => receivedNotification = notification;

        // Act
        notificationService.ShowErrorNotification("Test error message");

        // Assert
        Assert.NotNull(receivedNotification);
        Assert.Equal(NotificationType.Error, receivedNotification.Type);
        Assert.Equal("Test error message", receivedNotification.Message);
    }

    [Fact]
    public void ClearNotification_ShouldTriggerOnClearNotificationEvent()
    {
        // Arrange
        var notificationService = new NotificationService();
        var eventTriggered = false;
        notificationService.OnClearNotification += () => eventTriggered = true;

        // Act
        notificationService.ClearNotification();

        // Assert
        Assert.True(eventTriggered);
    }

    [Fact]
    public void ShowWarningNotification_ShouldTriggerOnNotificationEvent()
    {
        // Arrange
        var notificationService = new NotificationService();
        Notification? receivedNotification = null;
        notificationService.OnNotification += (notification) => receivedNotification = notification;

        // Act
        notificationService.ShowWarningNotification("Test warning message");

        // Assert
        Assert.NotNull(receivedNotification);
        Assert.Equal(NotificationType.Warning, receivedNotification.Type);
        Assert.Equal("Test warning message", receivedNotification.Message);
    }

    [Fact]
    public void ShowInfoNotification_ShouldTriggerOnNotificationEvent()
    {
        // Arrange
        var notificationService = new NotificationService();
        Notification? receivedNotification = null;
        notificationService.OnNotification += (notification) => receivedNotification = notification;

        // Act
        notificationService.ShowInfoNotification("Test info message");

        // Assert
        Assert.NotNull(receivedNotification);
        Assert.Equal(NotificationType.Info, receivedNotification.Type);
        Assert.Equal("Test info message", receivedNotification.Message);
    }
}