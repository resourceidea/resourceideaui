using EastSeat.ResourceIdea.Application.Features.Engagements.Commands;
using EastSeat.ResourceIdea.Domain.Common.ValueObjects;

using FluentValidation;

namespace EastSeat.ResourceIdea.Application.Features.Engagements.Validators;

/// <summary>
/// Validator for the StartEngagementCommand.
/// </summary>
public sealed class StartEngagementCommandValidator : AbstractValidator<StartEngagementCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StartEngagementCommandValidator"/> class.
    /// </summary>
    public StartEngagementCommandValidator()
    {
        RuleFor(engagement => engagement.CommencementDate)
            .Must(BeValidCommencementDate)
            .WithMessage($"The commencement date must be a valid date.");
    }

    /// <summary>
    /// Checks if the commencement date is valid.
    /// </summary>
    /// <param name="commencementDate">The commencement date to validate.</param>
    /// <returns>True if the commencement date is valid; otherwise, false.</returns>
    private bool BeValidCommencementDate(DateTimeOffset commencementDate)
    {
        return commencementDate != new EmptyDate() && commencementDate <= DateTimeOffset.Now;
    }
}
