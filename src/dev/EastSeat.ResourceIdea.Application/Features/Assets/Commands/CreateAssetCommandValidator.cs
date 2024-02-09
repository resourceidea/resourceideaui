using FluentValidation;

namespace EastSeat.ResourceIdea.Application.Features.Asset.Commands;

/// <summary>
/// Validates the command to create an asset.
/// </summary>
public class CreateAssetCommandValidator : AbstractValidator<CreateAssetCommand>
{
    /// <summary>
    /// Instantiates <see cref="CreateAssetCommandValidator"/>.
    /// </summary>
    public CreateAssetCommandValidator()
    {
        RuleFor(asset => asset.Description)
            .NotEmpty().WithMessage("Asset description is required.")
            .NotNull();
    }
}
