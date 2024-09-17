using System;
using System.Data;
using EastSeat.ResourceIdea.Application.Features.ApplicationUsers.Commands;
using FluentValidation;

namespace EastSeat.ResourceIdea.Application.Features.ApplicationUsers.Validators;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Email is not valid.");
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.");
    }
}
