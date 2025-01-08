using EastSeat.ResourceIdea.Application.Features.EngagementTasks.Commands;

using FluentValidation;

namespace EastSeat.ResourceIdea.Application.Features.EngagementTasks.Validators;

public sealed class CreateEngagementTaskCommandValidator : AbstractValidator<CreateEngagementTaskCommand>
{
    public CreateEngagementTaskCommandValidator()
    {
        RuleFor(c => c.Title)
            .NotEmpty().WithMessage("Engagement task title is required")
            .MaximumLength(100).WithMessage("Engagement task title must not exceed 100 characters");

        RuleFor(c => c.Description)
            .MaximumLength(200).WithMessage("Engagement task description must not exceed 200 characters");

        RuleFor(c => c.DueDate)
            .Must(d => d.HasValue && d.Value > DateTimeOffset.UtcNow).WithMessage("Due date must be in the future");
    }
}