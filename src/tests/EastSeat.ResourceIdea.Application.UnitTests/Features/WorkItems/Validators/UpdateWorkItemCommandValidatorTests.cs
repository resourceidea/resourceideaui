using System;
using EastSeat.ResourceIdea.Application.Features.WorkItems.Commands;
using EastSeat.ResourceIdea.Application.Features.WorkItems.Validators;
using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.WorkItems.ValueObjects;
using Xunit;

namespace EastSeat.ResourceIdea.Application.UnitTests.Features.WorkItems.Validators;

/// <summary>
/// Unit tests for <see cref="UpdateWorkItemCommandValidator"/> business rules.
/// </summary>
public class UpdateWorkItemCommandValidatorTests
{
    private readonly UpdateWorkItemCommandValidator _validator;

    public UpdateWorkItemCommandValidatorTests()
    {
        _validator = new UpdateWorkItemCommandValidator();
    }

    [Fact]
    public void Validate_WhenValidCommand_ShouldReturnValid()
    {
        // Arrange
        var command = new UpdateWorkItemCommand
        {
            WorkItemId = WorkItemId.Create(Guid.NewGuid()),
            EngagementId = EngagementId.Create(Guid.NewGuid()),
            Title = "Test Work Item",
            Description = "Test description",
            Status = WorkItemStatus.NotStarted,
            Priority = Priority.Medium,
            StartDate = DateTimeOffset.UtcNow,
            CompletedDate = DateTimeOffset.UtcNow.AddDays(5)
        };
        command.TenantId = TenantId.Create(Guid.NewGuid());

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Validate_WhenTitleIsEmpty_ShouldReturnInvalid()
    {
        // Arrange
        var command = new UpdateWorkItemCommand
        {
            WorkItemId = WorkItemId.Create(Guid.NewGuid()),
            EngagementId = EngagementId.Create(Guid.NewGuid()),
            Title = "",
            Status = WorkItemStatus.NotStarted,
            Priority = Priority.Medium
        };
        command.TenantId = TenantId.Create(Guid.NewGuid());

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == "Title is required.");
    }

    [Fact]
    public void Validate_WhenPriorityIsInvalidEnum_ShouldReturnInvalid()
    {
        // Arrange - Create command with valid enum value first
        var command = new UpdateWorkItemCommand
        {
            WorkItemId = WorkItemId.Create(Guid.NewGuid()),
            EngagementId = EngagementId.Create(Guid.NewGuid()),
            Title = "Test Work Item",
            Status = WorkItemStatus.NotStarted,
            Priority = (Priority)999 // Invalid enum value
        };
        command.TenantId = TenantId.Create(Guid.NewGuid());

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == "Priority must be a valid priority level.");
    }

    [Theory]
    [InlineData(WorkItemStatus.InProgress)]
    [InlineData(WorkItemStatus.OnHold)]
    [InlineData(WorkItemStatus.Completed)]
    [InlineData(WorkItemStatus.Canceled)]
    public void Validate_WhenStartDateIsSetAndStatusIsNotNotStarted_ShouldReturnInvalid(WorkItemStatus status)
    {
        // Arrange
        var command = new UpdateWorkItemCommand
        {
            WorkItemId = WorkItemId.Create(Guid.NewGuid()),
            EngagementId = EngagementId.Create(Guid.NewGuid()),
            Title = "Test Work Item",
            Status = status,
            Priority = Priority.Medium,
            StartDate = DateTimeOffset.UtcNow
        };
        command.TenantId = TenantId.Create(Guid.NewGuid());

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == "Start date can only be edited when status is NotStarted.");
    }

    [Theory]
    [InlineData(WorkItemStatus.Completed)]
    [InlineData(WorkItemStatus.Canceled)]
    public void Validate_WhenEndDateIsSetAndStatusIsInvalid_ShouldReturnInvalid(WorkItemStatus status)
    {
        // Arrange
        var command = new UpdateWorkItemCommand
        {
            WorkItemId = WorkItemId.Create(Guid.NewGuid()),
            EngagementId = EngagementId.Create(Guid.NewGuid()),
            Title = "Test Work Item",
            Status = status,
            Priority = Priority.Medium,
            CompletedDate = DateTimeOffset.UtcNow
        };
        command.TenantId = TenantId.Create(Guid.NewGuid());

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == "End date can only be edited when status is NotStarted, InProgress or OnHold.");
    }

    [Theory]
    [InlineData(WorkItemStatus.NotStarted)]
    [InlineData(WorkItemStatus.InProgress)]
    [InlineData(WorkItemStatus.OnHold)]
    public void Validate_WhenEndDateIsSetAndStatusIsValid_ShouldReturnValid(WorkItemStatus status)
    {
        // Arrange
        var command = new UpdateWorkItemCommand
        {
            WorkItemId = WorkItemId.Create(Guid.NewGuid()),
            EngagementId = EngagementId.Create(Guid.NewGuid()),
            Title = "Test Work Item",
            Status = status,
            Priority = Priority.Medium,
            CompletedDate = DateTimeOffset.UtcNow
        };
        command.TenantId = TenantId.Create(Guid.NewGuid());

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_WhenStartDateIsNullAndStatusIsNotNotStarted_ShouldReturnValid()
    {
        // Arrange
        var command = new UpdateWorkItemCommand
        {
            WorkItemId = WorkItemId.Create(Guid.NewGuid()),
            EngagementId = EngagementId.Create(Guid.NewGuid()),
            Title = "Test Work Item",
            Status = WorkItemStatus.InProgress,
            Priority = Priority.Medium,
            StartDate = null
        };
        command.TenantId = TenantId.Create(Guid.NewGuid());

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_WhenEndDateIsNullAndStatusIsInvalid_ShouldReturnValid()
    {
        // Arrange
        var command = new UpdateWorkItemCommand
        {
            WorkItemId = WorkItemId.Create(Guid.NewGuid()),
            EngagementId = EngagementId.Create(Guid.NewGuid()),
            Title = "Test Work Item",
            Status = WorkItemStatus.Completed,
            Priority = Priority.Medium,
            CompletedDate = null
        };
        command.TenantId = TenantId.Create(Guid.NewGuid());

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.True(result.IsValid);
    }
}