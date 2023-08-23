using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Asset.Commands;

/// <summary>
/// Command to create an asset record.
/// </summary>
public class CreateAssetCommand : IRequest<CreateAssetCommandResponse>
{
    /// <summary>Asset ID.</summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>Asset description.</summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>Subscription ID. Empty if the value is not set.</summary>
    public Guid SubscriptionId { get; set; } = Guid.Empty;
}
