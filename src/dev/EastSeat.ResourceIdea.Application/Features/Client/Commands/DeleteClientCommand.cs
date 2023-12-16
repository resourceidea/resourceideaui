using EastSeat.ResourceIdea.Application.Responses;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Client.Commands;

public class DeleteClientCommand : IRequest<BaseResponse<Unit>>
{
    /// <summary>Client ID.</summary>
    public Guid Id { get; set; }

    /// <summary>User who triggered the command to delete a client.</summary>
    public string LastModifiedBy { get; set; } = string.Empty;
}
