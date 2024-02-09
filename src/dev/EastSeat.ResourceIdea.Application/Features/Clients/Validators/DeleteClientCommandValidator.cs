using EastSeat.ResourceIdea.Application.Features.Clients.Commands;

using FluentValidation;

namespace EastSeat.ResourceIdea.Application.Features.Clients.Validators;

public class DeleteClientCommandValidator : AbstractValidator<DeleteClientCommand>
{
    public DeleteClientCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("Client Id is required.");
    }
}
