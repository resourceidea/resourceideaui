using EastSeat.ResourceIdea.Application.Features.WorkItems.Commands;
using EastSeat.ResourceIdea.Application.Features.WorkItems.Validators;
using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

using FluentValidation.TestHelper;

namespace EastSeat.ResourceIdea.Application.UnitTests.Features.WorkItems.Validators;

/// <summary>
/// Unit tests for <see cref="CreateWorkItemCommandValidator"/>.
/// </summary>
public class CreateWorkItemCommandValidatorTests
{
    private readonly CreateWorkItemCommandValidator _validator;

    public CreateWorkItemCommandValidatorTests()
    {
        _validator = new CreateWorkItemCommandValidator();
    }

    [Fact]
    public void Validate_WhenTitleIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateWorkItemCommand
        {
            Title = string.Empty,
            EngagementId = EngagementId.Create(Guid.NewGuid()),
            TenantId = TenantId.Create(Guid.NewGuid())
        };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage("Title is required.");
    }

    [Fact]
    public void Validate_WhenTitleIsNull_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateWorkItemCommand
        {
            Title = null!,
            EngagementId = EngagementId.Create(Guid.NewGuid()),
            TenantId = TenantId.Create(Guid.NewGuid())
        };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Validate_WhenEngagementIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateWorkItemCommand
        {
            Title = "Test Work Item",
            EngagementId = EngagementId.Empty,
            TenantId = TenantId.Create(Guid.NewGuid())
        };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.EngagementId)
            .WithErrorMessage("Engagement ID is required.");
    }

    [Fact]
    public void Validate_WhenTenantIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateWorkItemCommand
        {
            Title = "Test Work Item",
            EngagementId = EngagementId.Create(Guid.NewGuid()),
            TenantId = TenantId.Empty
        };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.TenantId)
            .WithErrorMessage("Tenant ID is required.");
    }

    [Fact]
    public void Validate_WhenStartDateIsInPast_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateWorkItemCommand
        {
            Title = "Test Work Item",
            EngagementId = EngagementId.Create(Guid.NewGuid()),
            TenantId = TenantId.Create(Guid.NewGuid()),
            StartDate = DateTimeOffset.UtcNow.AddDays(-1)
        };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.StartDate)
            .WithErrorMessage("Start date must be today or in the future.");
    }

    [Fact]
    public void Validate_WhenStartDateIsTodayOrFuture_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new CreateWorkItemCommand
        {
            Title = "Test Work Item",
            EngagementId = EngagementId.Create(Guid.NewGuid()),
            TenantId = TenantId.Create(Guid.NewGuid()),
            StartDate = DateTimeOffset.UtcNow.AddDays(1)
        };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.StartDate);
    }

    [Fact]
    public void Validate_WhenStartDateIsNull_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new CreateWorkItemCommand
        {
            Title = "Test Work Item",
            EngagementId = EngagementId.Create(Guid.NewGuid()),
            TenantId = TenantId.Create(Guid.NewGuid()),
            StartDate = null
        };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.StartDate);
    }

    [Fact]
    public void Validate_WhenCompletedDateIsBeforeStartDate_ShouldHaveValidationError()
    {
        // Arrange
        var startDate = DateTimeOffset.UtcNow.AddDays(2);
        var command = new CreateWorkItemCommand
        {
            Title = "Test Work Item",
            EngagementId = EngagementId.Create(Guid.NewGuid()),
            TenantId = TenantId.Create(Guid.NewGuid()),
            StartDate = startDate,
            CompletedDate = startDate.AddDays(-1)
        };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.CompletedDate)
            .WithErrorMessage("Completed date cannot be earlier than the start date.");
    }

    [Fact]
    public void Validate_WhenCompletedDateIsAfterStartDate_ShouldNotHaveValidationError()
    {
        // Arrange
        var startDate = DateTimeOffset.UtcNow.AddDays(1);
        var command = new CreateWorkItemCommand
        {
            Title = "Test Work Item",
            EngagementId = EngagementId.Create(Guid.NewGuid()),
            TenantId = TenantId.Create(Guid.NewGuid()),
            StartDate = startDate,
            CompletedDate = startDate.AddDays(1)
        };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.CompletedDate);
    }

    [Fact]
    public void Validate_WhenCompletedDateIsNull_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new CreateWorkItemCommand
        {
            Title = "Test Work Item",
            EngagementId = EngagementId.Create(Guid.NewGuid()),
            TenantId = TenantId.Create(Guid.NewGuid()),
            StartDate = DateTimeOffset.UtcNow.AddDays(1),
            CompletedDate = null
        };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.CompletedDate);
    }

    [Fact]
    public void Validate_WhenPriorityIsValidEnum_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new CreateWorkItemCommand
        {
            Title = "Test Work Item",
            EngagementId = EngagementId.Create(Guid.NewGuid()),
            TenantId = TenantId.Create(Guid.NewGuid()),
            Priority = Priority.High
        };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Priority);
    }

    [Fact]
    public void Validate_WhenAllFieldsAreValid_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new CreateWorkItemCommand
        {
            Title = "Test Work Item",
            Description = "Test description",
            EngagementId = EngagementId.Create(Guid.NewGuid()),
            TenantId = TenantId.Create(Guid.NewGuid()),
            StartDate = DateTimeOffset.UtcNow.AddDays(1),
            CompletedDate = DateTimeOffset.UtcNow.AddDays(5),
            Priority = Priority.Medium
        };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}