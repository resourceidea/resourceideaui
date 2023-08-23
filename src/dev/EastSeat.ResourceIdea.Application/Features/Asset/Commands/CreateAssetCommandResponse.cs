using EastSeat.ResourceIdea.Application.Responses;

namespace EastSeat.ResourceIdea.Application.Features.Asset.Commands;

/// <summary>
/// Response from the command to create an asset.
/// </summary>
public class CreateAssetCommandResponse : BaseResponse
{
    public CreateAssetCommandResponse() : base()
    {
    }

    public CreateAssetDTO Asset { get; set; } = default!;
}
