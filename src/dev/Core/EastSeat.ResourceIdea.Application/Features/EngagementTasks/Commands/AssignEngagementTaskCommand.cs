﻿using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.EngagementTasks.Models;
using EastSeat.ResourceIdea.Domain.EngagementTasks.ValueObjects;
using EastSeat.ResourceIdea.Domain.Users.ValueObjects;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.EngagementTasks.Commands;

/// <summary>
/// Represents a command to assign an engagement task to a user.
/// </summary>
public sealed class AssignEngagementTaskCommand : IRequest<ResourceIdeaResponse<EngagementTaskModel>>
{
    /// <summary>
    /// Gets or sets the ID of the engagement task to be assigned.
    /// </summary>
    public required EngagementTaskId EngagementTaskId { get; init; }

    /// <summary>
    /// Gets or sets the ID of the user to whom the engagement task is assigned.
    /// </summary>
    public required ApplicationUserId ApplicationUserId { get; init; }
}