using EastSeat.ResourceIdea.Application.Features.Client.DTO;
using EastSeat.ResourceIdea.Application.Responses;
using EastSeat.ResourceIdea.Domain.Common;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Client.Commands;

/// <summary>
/// Command to create a client record.
/// </summary>
public class CreateClientCommand : IRequest<BaseResponse<ClientDTO>>
{
    /// <summary>Client Id.</summary>
    public Guid Id { get; set; }

    /// <summary>Subscription Id.</summary>
    public Guid SubscriptionId { get; set; }

    /// <summary>Client Name.</summary>
    public NonEmptyString Name { get; set; }

    /// <summary>Client Address.</summary>
    public NonEmptyString Address { get; set; }

    /// <summary>Client color code.</summary>
    public string? ColorCode { get; set; }

    /// <summary>User who triggered the command to create the client.</summary>
    public string? CreatedBy { get; set; }
}
