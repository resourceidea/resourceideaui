// ----------------------------------------------------------------------------------
// File: CreateWorkItemCommandValidator.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\WorkItems\Validators\CreateWorkItemCommandValidator.cs
// Description: Validator for the CreateWorkItemCommand.
// ----------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Application.Features.WorkItems.Commands;

using FluentValidation;

namespace EastSeat.ResourceIdea.Application.Features.WorkItems.Validators;

/// <summary>
/// Validator for the CreateWorkItemCommand.
/// </summary>
public sealed class CreateWorkItemCommandValidator : AbstractValidator<CreateWorkItemCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateWorkItemCommandValidator"/> class.
    /// </summary>
    public CreateWorkItemCommandValidator()
    {
        RuleFor(workItem => workItem.Title)
            .NotEmpty()
            .WithMessage("Title is required.");

        RuleFor(workItem => workItem.EngagementId)
            .Must(engagementId => !engagementId.IsEmpty())
            .WithMessage("Engagement ID is required.");

        RuleFor(workItem => workItem.TenantId)
            .Must(tenantId => !tenantId.IsEmpty())
            .WithMessage("Tenant ID is required.");

        RuleFor(workItem => workItem.StartDate)
            .Must(BeValidStartDate)
            .When(workItem => workItem.StartDate.HasValue)
            .WithMessage("Start date must be today or in the future.");

        RuleFor(workItem => workItem.CompletedDate)
            .Must((workItem, completedDate) => BeValidCompletedDate(workItem.StartDate, completedDate))
            .When(workItem => workItem.CompletedDate.HasValue)
            .WithMessage("Completed date cannot be earlier than the start date.");

        RuleFor(workItem => workItem.Priority)
            .InclusiveBetween(1, 5)
            .WithMessage("Priority must be between 1 and 5, where 1 is highest priority.");
    }

    /// <summary>
    /// Checks if the start date is valid.
    /// </summary>
    /// <param name="startDate">The start date to validate.</param>
    /// <returns>True if the start date is valid; otherwise, false.</returns>
    private bool BeValidStartDate(DateTimeOffset? startDate)
    {
        return startDate == null || startDate.Value.Date >= DateTimeOffset.UtcNow.Date;
    }

    /// <summary>
    /// Checks if the completed date is valid.
    /// </summary>
    /// <param name="startDate">The start date.</param>
    /// <param name="completedDate">The completed date to validate.</param>
    /// <returns>True if the completed date is valid; otherwise, false.</returns>
    private bool BeValidCompletedDate(DateTimeOffset? startDate, DateTimeOffset? completedDate)
    {
        if (completedDate == null) return true;
        if (startDate == null) return true;
        return completedDate.Value >= startDate.Value;
    }
}