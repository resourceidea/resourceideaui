using System.Security.Claims;
using Microsoft.Extensions.Options;

namespace EastSeat.ResourceIdea.Application.Models;

/// <summary>
/// Application user view model.
/// </summary>
public class ApplicationUserViewModel
{
    /// <summary>
    /// Application user's Id.
    /// </summary>
    public string? Id { get; set; }
    /// <summary>
    /// Application user's username.
    /// </summary>
    public string? UserName { get; set; }
    /// <summary>
    /// Application user's email.
    /// </summary>
    public string? Email { get; set; }
    /// <summary>
    /// Application user's first name.
    /// </summary>
    public string? FirstName { get; set; }
    /// <summary>
    /// Application user's last name.
    /// </summary>
    public string? LastName { get; set; }
    /// <summary>
    /// Application user's subscription Id.
    /// </summary>
    public Guid? SubscriptionId { get; set; }

    public ClaimsPrincipal ToClaimsPrincipal() => new(
        new ClaimsIdentity(new Claim[]
        {
            new (ClaimTypes.Name, UserName ?? string.Empty)
        }, "ResourceIdea"));

    public static ApplicationUserViewModel FromClaimsPrincipal(ClaimsPrincipal principal) => new()
    {
        UserName = principal.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty
    };
}
