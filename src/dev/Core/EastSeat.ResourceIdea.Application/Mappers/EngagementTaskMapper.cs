using EastSeat.ResourceIdea.Application.Features.EngagementTasks.Commands;
using EastSeat.ResourceIdea.Domain.EngagementTasks.Entities;
using EastSeat.ResourceIdea.Domain.EngagementTasks.ValueObjects;
using EastSeat.ResourceIdea.Domain.Enums;

namespace EastSeat.ResourceIdea.Application.Mappers;

/// <summary>
/// Maps a CreateEngagementTaskCommand to an EngagementTask entity.
/// </summary>
public static class EngagementTaskMapper
{
    /// <summary>
    /// Converts a <see cref="CreateEngagementTaskCommand"/> to an <see cref="EngagementTask"/> entity.
    /// </summary>
    /// <param name="command">The command containing the details of the engagement task to create.</param>
    /// <returns>An <see cref="EngagementTask"/> entity populated with the details from the command.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the command is null.</exception>
    public static EngagementTask ToEntity(this CreateEngagementTaskCommand command)
    {
        ArgumentNullException.ThrowIfNull(command, nameof(command));

        return new EngagementTask
        {
            Id = EngagementTaskId.Create(Guid.NewGuid()),
            Description = command.Description,
            Title = command.Title,
            EngagementId = command.EngagementId,
            IsAssigned = false,
            Status = EngagementTaskStatus.NotStarted,
            DueDate = command.DueDate
        };
    }

    /// <summary>
    /// Converts a <see cref="UpdateEngagementTaskCommand"/> to an <see cref="EngagementTask"/> entity.
    /// </summary>
    /// <param name="command">The command containing the details of the engagement task to update.</param>
    /// <returns>An <see cref="EngagementTask"/> entity populated with the details from the command.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the command is null.</exception>
    public static EngagementTask ToEntity(this UpdateEngagementTaskCommand command)
    {
        ArgumentNullException.ThrowIfNull(command, nameof(command));

        return new EngagementTask
        {
            Id = command.EngagementTaskId,
            Description = command.Description,
            Title = command.Title,
            EngagementId = command.EngagementId,
            IsAssigned = command.IsAssigned,
            Status = command.EngagementTaskStatus,
            DueDate = command.DueDate
        };
    }
}
