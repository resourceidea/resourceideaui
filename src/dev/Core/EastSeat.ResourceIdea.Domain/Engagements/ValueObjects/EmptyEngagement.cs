using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Common.ValueObjects;
using EastSeat.ResourceIdea.Domain.Engagements.Entities;

namespace EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;

public sealed record EmptyEngagement
{
    /// <summary>
    /// Instance of an empty <see cref="Engagement"/>.
    /// </summary>
    public static Engagement Instance => new()
    {
        Id = EngagementId.Empty,
        ClientId = ClientId.Empty,
        CommencementDate = EmptyDate.Value,
        CompletionDate = EmptyDate.Value
    };

    private EmptyEngagement() { }
}