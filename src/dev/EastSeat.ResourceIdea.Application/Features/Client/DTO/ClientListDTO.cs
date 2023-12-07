namespace EastSeat.ResourceIdea.Application.Features.Client.DTO;

/// <summary>
/// Data Transfer Object for the list of clients.
/// </summary>
public record ClientListDTO
{
    /// <summary>Client ID.</summary>
    public Guid Id { get; set; }

    /// <summary>Client name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Subscription ID.</summary>
    public Guid SubscriptionId { get; set; }
}
