using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Domain.Common.Responses;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.Models;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Queries;

/// <summary>
/// Query to get a list of subscription services.
/// </summary>
public sealed class GetSubscriptionServicesListQuery : IRequest<ResourceIdeaResponse<PagedList<SubscriptionServiceModel>>>
{
    /// <summary>Current page number of the subscription services paged list.</summary>
    public int CurrentPageNumber { get; set; } = 1;

    /// <summary>Size of the subscription services paged list.</summary>
    public int PageSize { get; set; } = 10;

    /// <summary>Subscription services query filter.</summary>
    public string Filter { get; set; } = string.Empty;
}
