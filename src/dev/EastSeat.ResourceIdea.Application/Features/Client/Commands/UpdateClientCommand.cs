using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EastSeat.ResourceIdea.Application.Features.Client.DTO;
using EastSeat.ResourceIdea.Application.Responses;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Client.Commands;

public class UpdateClientCommand : IRequest<BaseResponse<ClientDTO>>
{
    /// <summary>Client Id.</summary>
    public Guid Id { get; set; }

    /// <summary>Subscription Id.</summary>
    public Guid SubscriptionId { get; set; }

    /// <summary>Client Name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Client Address.</summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>Client color code.</summary>
    public string ColorCode { get; set; } = string.Empty;
}
