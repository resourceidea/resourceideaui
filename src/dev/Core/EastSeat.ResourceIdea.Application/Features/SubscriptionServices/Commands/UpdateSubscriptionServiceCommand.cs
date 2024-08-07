using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.Models;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.ValueObjects;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Commands;
/// <summary>
/// Command to update a subscription service.
/// </summary>
public sealed record UpdateSubscriptionServiceCommand : IRequest<ResourceIdeaResponse<SubscriptionServiceModel>>
{
    /// <summary>
    /// Gets the identifier of the subscription service to update.
    /// </summary>
    public SubscriptionServiceId Id { get; init; }

    /// <summary>
    /// Gets the new name of the subscription service.
    /// </summary>
    public string Name { get; init; } = string.Empty;
}