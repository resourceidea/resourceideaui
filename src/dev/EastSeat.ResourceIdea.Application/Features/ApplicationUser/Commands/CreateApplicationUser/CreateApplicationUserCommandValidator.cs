using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;

namespace EastSeat.ResourceIdea.Application.Features.ApplicationUser.Commands.CreateApplicationUser;

/// <summary>
/// Validates the command used to create an application user.
/// </summary>
public class CreateApplicationUserCommandValidator : AbstractValidator<CreateApplicationUserCommand>
{
    public CreateApplicationUserCommandValidator()
    {
        RuleFor(applicationUser => applicationUser.FirstName)
            .NotEmpty().WithMessage(Constants.ErrorCodes.MissingFirstName)
            .NotNull();

        RuleFor(applicationUser => applicationUser.LastName)
            .NotEmpty().WithMessage(Constants.ErrorCodes.MissingLastName)
            .NotNull();

        RuleFor(subscription => subscription.SubscriptionId)
            .NotEqual(Guid.Empty).WithMessage(Constants.ErrorCodes.MissingSubscriptionId);

        RuleFor(subscription => subscription.Email)
            .NotEmpty().WithMessage(Constants.ErrorCodes.MissingEmail)
            .NotNull();

        RuleFor(applicationUser => applicationUser.Password)
            .NotEmpty().WithMessage(Constants.ErrorCodes.MissingPassword)
            .NotNull();
    }
}
