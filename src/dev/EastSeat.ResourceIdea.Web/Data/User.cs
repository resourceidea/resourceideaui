using System.Security.Claims;

namespace EastSeat.ResourceIdea.Web.Data;

public class User
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public ClaimsPrincipal ToClaimsPrincipal() => new(
        new ClaimsIdentity(new Claim[]
        {
            new (ClaimTypes.Name, Username),
            new (ClaimTypes.Hash, Password)
        }, "ResourceIdea"));

    public static User FromClaimsPrincipal(ClaimsPrincipal principal) => new()
    {
        Username = principal.FindFirstValue(ClaimTypes.Name) ?? string.Empty,
        Password = principal.FindFirstValue(ClaimTypes.Hash) ?? string.Empty
    };
}
