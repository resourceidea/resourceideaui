namespace EastSeat.ResourceIdea.Domain.Roles.Models;

/// <summary>
/// Role claim model.
/// </summary>
public record RoleClaimModel
{
    /// <summary>
    /// Claim identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Role identifier this claim belongs to.
    /// </summary>
    public required string RoleId { get; set; }

    /// <summary>
    /// Claim type.
    /// </summary>
    public required string ClaimType { get; set; }

    /// <summary>
    /// Claim value.
    /// </summary>
    public required string ClaimValue { get; set; }

    /// <summary>
    /// Description of what this claim allows.
    /// </summary>
    public string? Description { get; set; }
}