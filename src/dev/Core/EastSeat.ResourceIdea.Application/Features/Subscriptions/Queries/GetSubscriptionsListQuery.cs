using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;

namespace EastSeat.ResourceIdea.Application.Features.Subscriptions.Queries;

/// <summary>
/// Query to get a list of subscriptions.
/// </summary>
public sealed class GetSubscriptionsListQuery
{
    public required PagedListRequest Request { get; set; }
}
