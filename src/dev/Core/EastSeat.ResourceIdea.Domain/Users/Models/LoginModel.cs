namespace EastSeat.ResourceIdea.Domain.Users.Models;

/// <summary>
/// Model for logging in a user.
/// </summary>
public class LoginModel
{
    /// <summary>Email of the user.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Password of the user.</summary>
    public string Password { get; set; } = string.Empty;
}
