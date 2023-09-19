namespace EastSeat.ResourceIdea.Application.Models;

/// <summary>
/// Represents a response to an authentication response.
/// </summary>
public class AuthenticationResponse
{
    /// <summary>User ID.</summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>Username.</summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>User email.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Authentication token.</summary>
    public string Token { get; set; } = string.Empty;
}
