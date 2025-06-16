using System;
using EastSeat.ResourceIdea.Application.Features.WorkItems.Commands;
using EastSeat.ResourceIdea.Application.Mappers;
using EastSeat.ResourceIdea.Domain.Employees.ValueObjects;
using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.WorkItems.ValueObjects;
using Xunit;

namespace EastSeat.ResourceIdea.Application.UnitTests.Features.WorkItems.Commands;

/// <summary>
/// Unit tests for <see cref="UpdateWorkItemCommand"/> mapper methods.
/// </summary>
public class UpdateWorkItemCommandTests
{
    [Fact]
    public void ToEntity_WhenValidCommand_ShouldReturnWorkItemWithCorrectValues()
    {
        // Arrange
        var workItemId = WorkItemId.Create(Guid.NewGuid());
        var tenantId = TenantId.Create(Guid.NewGuid());
        var engagementId = EngagementId.Create(Guid.NewGuid());
        var assignedToId = EmployeeId.Create(Guid.NewGuid());
        var startDate = DateTimeOffset.UtcNow;
        var completedDate = DateTimeOffset.UtcNow.AddDays(5);

        var command = new UpdateWorkItemCommand
        {
            WorkItemId = workItemId,
            TenantId = tenantId,
            EngagementId = engagementId,
            Title = "Test Work Item",
            Description = "Test description",
            Status = WorkItemStatus.InProgress,
            Priority = 2,
            AssignedToId = assignedToId,
            StartDate = startDate,
            CompletedDate = completedDate
        };

        // Act
        var workItem = command.ToEntity();

        // Assert
        Assert.Equal(workItemId, workItem.Id);
        Assert.Equal(tenantId, workItem.TenantId);
        Assert.Equal(engagementId, workItem.EngagementId);
        Assert.Equal("Test Work Item", workItem.Title);
        Assert.Equal("Test description", workItem.Description);
        Assert.Equal(WorkItemStatus.InProgress, workItem.Status);
        Assert.Equal(2, workItem.Priority);
        Assert.Equal(assignedToId, workItem.AssignedToId);
        Assert.Equal(startDate, workItem.StartDate);
        Assert.Equal(completedDate, workItem.CompletedDate);
    }

    [Fact]
    public void ToEntity_WhenNullDescription_ShouldReturnWorkItemWithNullDescription()
    {
        // Arrange
        var command = new UpdateWorkItemCommand
        {
            WorkItemId = WorkItemId.Create(Guid.NewGuid()),
            TenantId = TenantId.Create(Guid.NewGuid()),
            EngagementId = EngagementId.Create(Guid.NewGuid()),
            Title = "Test Work Item",
            Description = null,
            Status = WorkItemStatus.NotStarted,
            Priority = 3
        };

        // Act
        var workItem = command.ToEntity();

        // Assert
        Assert.Null(workItem.Description);
    }

    [Fact]
    public void ToEntity_WhenNullDates_ShouldReturnWorkItemWithNullDates()
    {
        // Arrange
        var command = new UpdateWorkItemCommand
        {
            WorkItemId = WorkItemId.Create(Guid.NewGuid()),
            TenantId = TenantId.Create(Guid.NewGuid()),
            EngagementId = EngagementId.Create(Guid.NewGuid()),
            Title = "Test Work Item",
            Status = WorkItemStatus.NotStarted,
            Priority = 3,
            StartDate = null,
            CompletedDate = null
        };

        // Act
        var workItem = command.ToEntity();

        // Assert
        Assert.Null(workItem.StartDate);
        Assert.Null(workItem.CompletedDate);
    }

    [Fact]
    public void ToEntity_WhenNullAssignedToId_ShouldReturnWorkItemWithNullAssignedToId()
    {
        // Arrange
        var command = new UpdateWorkItemCommand
        {
            WorkItemId = WorkItemId.Create(Guid.NewGuid()),
            TenantId = TenantId.Create(Guid.NewGuid()),
            EngagementId = EngagementId.Create(Guid.NewGuid()),
            Title = "Test Work Item",
            Status = WorkItemStatus.NotStarted,
            Priority = 3,
            AssignedToId = null
        };

        // Act
        var workItem = command.ToEntity();

        // Assert
        Assert.Null(workItem.AssignedToId);
    }
}