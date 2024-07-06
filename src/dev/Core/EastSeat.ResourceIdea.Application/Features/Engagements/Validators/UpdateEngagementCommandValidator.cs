﻿using EastSeat.ResourceIdea.Application.Features.Engagements.Commands;

using FluentValidation;

namespace EastSeat.ResourceIdea.Application.Features.Engagements.Validators;

/// <summary>
/// Validator for updating an engagement command.
/// </summary>
public sealed class UpdateEngagementCommandValidator : AbstractValidator<UpdateEngagementCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateEngagementCommandValidator"/> class.
    /// </summary>
    public UpdateEngagementCommandValidator()
    {
        RuleFor(engagement => engagement.CommencementDate)
            .LessThanOrEqualTo(DateTime.Now)
            .WithMessage("Invalid commencement date.");

        RuleFor(engagement => engagement.CompletionDate)
            .GreaterThanOrEqualTo(DateTimeOffset.Now)
            .WithMessage("Invalid completion date.");
    }
}
