using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Domain.Subscriptions.Models;
using EastSeat.ResourceIdea.Domain.Types;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Subscriptions.Queries;

/// <summary>
/// Query to get a list of subscriptions.
/// </summary>
public sealed class GetSubscriptionsListQuery : IRequest<ResourceIdeaResponse<PagedListResponse<SubscriptionModel>>>
{
    /// <summary>
    /// Query request.
    /// </summary>
    public required PagedListRequest Query { get; set; }
}
