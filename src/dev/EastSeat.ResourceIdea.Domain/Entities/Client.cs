using EastSeat.ResourceIdea.Domain.Common;

namespace EastSeat.ResourceIdea.Domain.Entities;

/// <summary>
/// Subscriber's client profile.
/// </summary>
public class Client
{
    /// <summary>Client ID.</summary>
    public Guid Id { get; set; } = Guid.Empty;

    /// <summary>Client name.</summary>
    public string? Name { get; set; }

    /// <summary>Client physical address.</summary>
    public string? Address { get; set; }

    /// <summary>Color code to identify client engagements.</summary>
    public string? ColorCode { get; set; }
}
