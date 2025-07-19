using EastSeat.ResourceIdea.Application.Features.WorkItems.Commands;
using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

namespace EastSeat.ResourceIdea.Application.UnitTests.Features.WorkItems.Commands;

/// <summary>
/// Unit tests for <see cref="CreateWorkItemCommand"/> validation.
/// </summary>
public class CreateWorkItemCommandTests
{
    [Fact]
    public void Validate_WhenValidCommand_ShouldReturnValidResponse()
    {
        // Arrange
        var command = new CreateWorkItemCommand
        {
            Title = "Test Work Item",
            Description = "Test description",
            EngagementId = EngagementId.Create(Guid.NewGuid()),
            TenantId = TenantId.Create(Guid.NewGuid()),
            StartDate = DateTimeOffset.UtcNow.AddDays(1),
            Priority = Priority.Medium
        };

        // Act
        var result = command.Validate();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.ValidationFailureMessages);
    }

    [Fact]
    public void Validate_WhenTitleIsEmpty_ShouldReturnInvalidResponse()
    {
        // Arrange
        var command = new CreateWorkItemCommand
        {
            Title = string.Empty,
            EngagementId = EngagementId.Create(Guid.NewGuid()),
            TenantId = TenantId.Create(Guid.NewGuid())
        };

        // Act
        var result = command.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.ValidationFailureMessages, msg => msg.Contains("Title"));
    }

    [Fact]
    public void Validate_WhenStartDateIsInPast_ShouldReturnInvalidResponse()
    {
        // Arrange
        var command = new CreateWorkItemCommand
        {
            Title = "Test Work Item",
            EngagementId = EngagementId.Create(Guid.NewGuid()),
            TenantId = TenantId.Create(Guid.NewGuid()),
            StartDate = DateTimeOffset.UtcNow.AddDays(-1)
        };

        // Act
        var result = command.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.ValidationFailureMessages, msg => msg.Contains("Start date"));
    }

    [Fact]
    public void Validate_WhenCompletedDateIsBeforeStartDate_ShouldReturnInvalidResponse()
    {
        // Arrange
        var command = new CreateWorkItemCommand
        {
            Title = "Test Work Item",
            EngagementId = EngagementId.Create(Guid.NewGuid()),
            TenantId = TenantId.Create(Guid.NewGuid()),
            StartDate = DateTimeOffset.UtcNow.AddDays(2),
            CompletedDate = DateTimeOffset.UtcNow.AddDays(1)
        };

        // Act
        var result = command.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.ValidationFailureMessages, msg => msg.Contains("Completed date"));
    }

    [Fact]
    public void ToEntity_WhenCalled_ShouldCreateWorkItemWithCorrectProperties()
    {
        // Arrange
        var engagementId = EngagementId.Create(Guid.NewGuid());
        var tenantId = TenantId.Create(Guid.NewGuid());
        var startDate = DateTimeOffset.UtcNow.AddDays(1);

        var command = new CreateWorkItemCommand
        {
            Title = "Test Work Item",
            Description = "Test description",
            EngagementId = engagementId,
            TenantId = tenantId,
            StartDate = startDate,
            Priority = Priority.High
        };

        // Act
        var workItem = command.ToEntity();

        // Assert
        Assert.Equal(command.Title, workItem.Title);
        Assert.Equal(command.Description, workItem.Description);
        Assert.Equal(command.EngagementId, workItem.EngagementId);
        Assert.Equal(command.TenantId, workItem.TenantId);
        Assert.Equal(command.StartDate, workItem.PlannedStartDate);
        Assert.Equal(command.Priority, workItem.Priority);
        Assert.Equal(WorkItemStatus.NotStarted, workItem.Status);
        Assert.False(workItem.Id.IsEmpty());
    }
}