using EastSeat.ResourceIdea.Application.Extensions;
using EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Commands;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.Entities;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.ValueObjects;

namespace EastSeat.ResourceIdea.Application.Mappers;

/// <summary>
/// Provides extension methods for mapping subscription service commands to entities.
/// </summary>
public static class SubscriptionServiceMapper
{
    /// <summary>
    /// Maps a <see cref="CreateSubscriptionServiceCommand"/> to a <see cref="SubscriptionService"/> entity.
    /// </summary>
    /// <param name="command">The command to map from.</param>
    /// <returns>A new instance of <see cref="SubscriptionService"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the command is null.</exception>
    public static SubscriptionService ToEntity(this CreateSubscriptionServiceCommand command)
    {
        ArgumentNullException.ThrowIfNull(command, nameof(command));
        command.Name.ThrowIfNullOrEmptyOrWhiteSpace();

        return new SubscriptionService
        {
            Id = SubscriptionServiceId.Create(Guid.NewGuid()),
            Name = command.Name
        };
    }

    public static SubscriptionService ToEntity(this UpdateSubscriptionServiceCommand command)
    {
        ArgumentNullException.ThrowIfNull(command, nameof(command));
        command.Name.ThrowIfNullOrEmptyOrWhiteSpace();

        return new SubscriptionService
        {
            Id = command.SubscriptionServiceId,
            Name = command.Name
        };
    }
}
