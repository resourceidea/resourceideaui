using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Common.ValueObjects;
using EastSeat.ResourceIdea.Domain.Employees.ValueObjects;
using EastSeat.ResourceIdea.Domain.Engagements.Entities;
using EastSeat.ResourceIdea.Domain.Enums;

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
        StartDate = EmptyDate.Value,
        EndDate = EmptyDate.Value,
        Title = string.Empty,
        Description = string.Empty,
        EngagementStatus = EngagementStatus.NotStarted,
        ManagerId = EmployeeId.Empty,
        PartnerId = EmployeeId.Empty,
    };

    private EmptyEngagement() { }
}