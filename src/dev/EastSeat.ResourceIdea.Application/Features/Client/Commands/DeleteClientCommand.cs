using EastSeat.ResourceIdea.Application.Responses;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Client.Commands;

public class DeleteClientCommand : IRequest<BaseResponse<Unit>>
{
    /// <summary>Client ID.</summary>
    public Guid Id { get; set; }
}
