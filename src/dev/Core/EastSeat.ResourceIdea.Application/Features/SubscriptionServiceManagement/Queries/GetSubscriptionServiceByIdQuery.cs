namespace EastSeat.ResourceIdea.Application.Features.SubscriptionServiceManagement.Queries;

using EastSeat.ResourceIdea.Domain.Common.Responses;
using EastSeat.ResourceIdea.Domain.SubscriptionServiceManagement.Models;
using EastSeat.ResourceIdea.Domain.SubscriptionServiceManagement.ValueObjects;

using MediatR;

/// <summary>
/// Query to get a subscription service by its unique identifier.
/// </summary>
public sealed class GetSubscriptionServiceByIdQuery : IRequest<ResourceIdeaResponse<SubscriptionServiceModel>>
{
    /// <summary>
    /// Subscription service ID.
    /// </summary>
    public SubscriptionServiceId SubscriptionServiceId { get; set; }
}