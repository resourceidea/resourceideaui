namespace EastSeat.ResourceIdea.Application.Features.ApplicationUser.Commands.CreateApplicationUser;

/// <summary>
/// Application user that has been created.
/// </summary>
public class CreateApplicationUserViewModel
{
    /// <summary>Application user ID.</summary>
    public Guid UserId { get; set; } = Guid.Empty;

    /// <summary>Application user's first name.</summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>Application user's lat name.</summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>User's email</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Application user's subscription UserId.</summary>
    public Guid SubscriptionId { get; set; } = Guid.Empty;
}
