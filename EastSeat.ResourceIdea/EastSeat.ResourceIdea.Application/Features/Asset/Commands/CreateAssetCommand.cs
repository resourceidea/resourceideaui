using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Asset.Commands;

public class CreateAssetCommand : IRequest<CreateAssetCommandResponse>
{
    /// <summary>
    /// Asset description.
    /// </summary>
    public string Description { get; set; } = string.Empty;
}
