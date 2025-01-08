using EastSeat.ResourceIdea.Domain.SubscriptionServices.Models;
using EastSeat.ResourceIdea.Domain.Types;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Commands;

public sealed class CreateSubscriptionServiceCommand : IRequest<ResourceIdeaResponse<SubscriptionServiceModel>>
{
    /// <summary>Service name</summary>
    public string Name { get; set; } = string.Empty;
}