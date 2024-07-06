using EastSeat.ResourceIdea.Application.Features.Engagements.Commands;
using EastSeat.ResourceIdea.Domain.Common.ValueObjects;

using FluentValidation;

namespace EastSeat.ResourceIdea.Application.Features.Engagements.Validators
{
    public sealed class CompleteEngagementCommandValidator : AbstractValidator<CompleteEngagementCommand>
    {
        public CompleteEngagementCommandValidator()
        {
            RuleFor(engagement => engagement.CompletionDate)
                .Must(BeValidCompletionDate)
                .WithMessage($"The completion date must be a valid date.");
        }

        private bool BeValidCompletionDate(DateTimeOffset completionDate)
        {
            return completionDate != new EmptyDate() && completionDate > DateTimeOffset.Now;
        }
    }
}
