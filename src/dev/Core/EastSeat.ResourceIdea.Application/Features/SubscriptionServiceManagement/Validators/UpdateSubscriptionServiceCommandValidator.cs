using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EastSeat.ResourceIdea.Application.Features.SubscriptionServiceManagement.Commands;

using FluentValidation;

namespace EastSeat.ResourceIdea.Application.Features.SubscriptionServiceManagement.Validators;

public sealed class UpdateSubscriptionServiceCommandValidator : AbstractValidator<UpdateSubscriptionServiceCommand>
{
    public UpdateSubscriptionServiceCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Service name is required.");
    }
}
