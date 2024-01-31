using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EastSeat.ResourceIdea.Domain.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserSubscriptionId(this ClaimsPrincipal user)
    {
        const string subscriptionIdClaimType = "SubscriptionId";
        bool hasSubscriptionIdClaimType = Guid.TryParse(user.FindFirst(subscriptionIdClaimType)?.Value, out Guid subscriptionId);

        if (IsAuthenticatedUser(user) && hasSubscriptionIdClaimType)
        {
            return subscriptionId;
        }

        return Guid.Empty;
    }

    /// <summary>
    /// Get the user's Id.
    /// </summary>
    /// <param name="user">User</param>
    /// <returns>User Id</returns>
    /// TODO: Change the implementation to return PrimarySid claim type.
    public static string GetUserId(this ClaimsPrincipal user) => Guid.NewGuid().ToString();

    private static bool IsAuthenticatedUser(ClaimsPrincipal user) => user.Identity is not null && user.Identity.IsAuthenticated;
}
