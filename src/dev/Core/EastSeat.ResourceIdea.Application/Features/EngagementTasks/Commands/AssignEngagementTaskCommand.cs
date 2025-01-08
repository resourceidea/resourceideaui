using EastSeat.ResourceIdea.Domain.EngagementTasks.Models;
using EastSeat.ResourceIdea.Domain.EngagementTasks.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;
using EastSeat.ResourceIdea.Domain.Users.ValueObjects;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.EngagementTasks.Commands;

/// <summary>
/// Represents a command to assign an engagement task to a user.
/// </summary>
public sealed class AssignEngagementTaskCommand : IRequest<ResourceIdeaResponse<IReadOnlyList<EngagementTaskAssignmentModel>>>
{
    /// <summary>
    /// Gets or sets the ID of the engagement task to be assigned.
    /// </summary>
    public required EngagementTaskId EngagementTaskId { get; init; }

    /// <summary>
    /// Gets or sets the ID of the user to whom the engagement task is assigned.
    /// </summary>
    public required IReadOnlyList<ApplicationUserId> ApplicationUserIds { get; init; }

    /// <summary>
    /// Gets or sets the start date of the engagement task.
    /// </summary>
    public DateTimeOffset StartDate { get; init; }

    /// <summary>
    /// Gets or sets the end date of the engagement task.
    /// </summary>
    public DateTimeOffset EndDate { get; init; }
}