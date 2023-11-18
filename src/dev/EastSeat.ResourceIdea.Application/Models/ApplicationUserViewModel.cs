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
    public string Id { get; set; } = string.Empty;
    /// <summary>
    /// Application user's username.
    /// </summary>
    public string UserName { get; set; } = string.Empty;
    /// <summary>
    /// Application user's email.
    /// </summary>
    public string Email { get; set; } = string.Empty;
    /// <summary>
    /// Application user's first name.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;
    /// <summary>
    /// Application user's last name.
    /// </summary>
    public string LastName { get; set; } = string.Empty;
    /// <summary>
    /// Application user's subscription Id.
    /// </summary>
    public Guid SubscriptionId { get; set; } = Guid.Empty;
    /// <summary>
    /// Authentication token.
    /// </summary>
    public string Token { get; set; } = string.Empty;
    /// <summary>
    /// Authentication refresh token.
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;

    public ClaimsPrincipal ToClaimsPrincipal() => new(
        new ClaimsIdentity(new Claim[]
        {
            new (ClaimTypes.PrimarySid, Id),
            new (ClaimTypes.GivenName, FirstName),
            new (ClaimTypes.Surname, LastName),
            new (ClaimTypes.Name, UserName),
            new (ClaimTypes.Email, Email),
            new (ClaimTypes.GroupSid, SubscriptionId.ToString()),
            new ("Token", Token),
            new ("RefreshToken", Token)
        }, "ResourceIdea"));

    public static ApplicationUserViewModel FromClaimsPrincipal(ClaimsPrincipal principal) => new()
    {
        Id = principal.FindFirst(ClaimTypes.PrimarySid)?.Value ?? string.Empty,
        FirstName = principal.FindFirst(ClaimTypes.GivenName)?.Value ?? string.Empty,
        LastName = principal.FindFirst(ClaimTypes.Surname)?.Value ?? string.Empty,
        UserName = principal.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty,
        Email = principal.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty,
        SubscriptionId = new Guid(principal.FindFirst(ClaimTypes.GroupSid)?.Value ?? Guid.Empty.ToString()),
        Token = principal.FindFirst("Token")?.Value ?? string.Empty,
        RefreshToken = principal.FindFirst("RefreshToken")?.Value ?? string.Empty
    };
}
