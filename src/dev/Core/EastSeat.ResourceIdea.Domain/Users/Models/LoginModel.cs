using System.ComponentModel.DataAnnotations;

namespace EastSeat.ResourceIdea.Domain.Users.Models;

/// <summary>
/// Model for logging in a user.
/// </summary>
public class LoginModel
{
    /// <summary>Email of the user.</summary>
    [Required(ErrorMessage = "Email address is required")]
    [EmailAddress]
    public string? Email { get; set; }

    /// <summary>Password of the user.</summary>
    [Required(ErrorMessage = "Password is required.")]
    public string? Password { get; set; }
}
