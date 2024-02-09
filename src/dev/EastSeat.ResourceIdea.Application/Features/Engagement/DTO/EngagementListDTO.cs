namespace EastSeat.ResourceIdea.Application.Features.Engagement.DTO;

public record EngagementListDTO
{
    /// <summary>Engagement ID.</summary>
    public Guid Id { get; set; }

    /// <summary>Engagement name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Engagement description.</summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>Client ID.</summary>
    public Guid? ClientId { get; set; }

    /// <summary>Date when the engagement is to start.</summary>
    public DateTime? StartDate { get; set; }

    /// <summary>Date when the engagement is to close.</summary>
    public DateTime? EndDate { get; set; }

    /// <summary>Current status of the engagement.</summary>
    public Constants.Engagement.Status Status { get; set; }
}
