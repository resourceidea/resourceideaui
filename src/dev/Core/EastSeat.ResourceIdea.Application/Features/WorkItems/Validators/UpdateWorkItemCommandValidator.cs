using EastSeat.ResourceIdea.Application.Features.WorkItems.Commands;
using EastSeat.ResourceIdea.Domain.Enums;

using FluentValidation;

namespace EastSeat.ResourceIdea.Application.Features.WorkItems.Validators;

/// <summary>
/// Validator for updating a work item command.
/// </summary>
public sealed class UpdateWorkItemCommandValidator : AbstractValidator<UpdateWorkItemCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateWorkItemCommandValidator"/> class.
    /// </summary>
    public UpdateWorkItemCommandValidator()
    {
        RuleFor(workItem => workItem.Title)
            .NotEmpty()
            .WithMessage("Title is required.");

        RuleFor(workItem => workItem.Priority)
            .IsInEnum()
            .WithMessage("Priority must be a valid priority level.");

        // Business rule: Start date cannot be edited when status is not NotStarted
        RuleFor(workItem => workItem.StartDate)
            .Must((workItem, startDate) => workItem.Status == WorkItemStatus.NotStarted || startDate == null)
            .WithMessage("Start date can only be edited when status is NotStarted.");

        // Business rule: End date can only be edited when status is NotStarted, InProgress or OnHold
        RuleFor(workItem => workItem.CompletedDate)
            .Must((workItem, completedDate) => 
                workItem.Status == WorkItemStatus.NotStarted || 
                workItem.Status == WorkItemStatus.InProgress || 
                workItem.Status == WorkItemStatus.OnHold ||
                completedDate == null)
            .WithMessage("End date can only be edited when status is NotStarted, InProgress or OnHold.");
    }
}