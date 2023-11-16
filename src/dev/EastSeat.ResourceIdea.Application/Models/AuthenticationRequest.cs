using System.ComponentModel.DataAnnotations;

namespace EastSeat.ResourceIdea.Application.Models;

/// <summary>
/// Represents a request for authentication to the service.
/// </summary>
public class AuthenticationRequest
{
    [Required]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
}
