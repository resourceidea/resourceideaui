using EastSeat.ResourceIdea.Application.Features.Client.Commands;

using FluentValidation;

namespace EastSeat.ResourceIdea.Application.Features.Client.Validators;

public class DeleteClientCommandValidator : AbstractValidator<DeleteClientCommand>
{
    public DeleteClientCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("Client Id is required.");
    }
}
