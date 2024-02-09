using EastSeat.ResourceIdea.Application.Features.Engagement.DTO;
using EastSeat.ResourceIdea.Domain.ValueObjects;

using MediatR;

using Optional;

namespace EastSeat.ResourceIdea.Application.Features.Engagement.Queries;

public class GetEngagementsListQuery : IRequest<PagedList<EngagementListDTO>>
{
    /// <summary>Engagements list page number.</summary>
    public int Page { get; set; } = 1;

    /// <summary>Engagements list page size.</summary>
    public int Size { get; set; } = 10;

    /// <summary>Filters engagements with name that contains the keywords.</summary>
    public Option<string> SearchKeyword { get; set; }

    /// <summary>Filters engagements with client id.</summary>
    public Option<Guid> ClientId { get; set; }

    /// <summary>Filters engagements with greater than or equal start dates.</summary>
    public Option<DateTime> StartDate { get; set; }

    /// <summary>Filters engagements with less than or equal end dates.</summary>
    public Option<DateTime> EndDate { get; set; }
}
