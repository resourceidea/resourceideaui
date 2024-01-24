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

    public static string GetUserId(this ClaimsPrincipal user) => 
        IsAuthenticatedUser(user) ? user.FindFirst(ClaimTypes.PrimarySid)?.Value ?? string.Empty : string.Empty;

    private static bool IsAuthenticatedUser(ClaimsPrincipal user) => user.Identity is not null && user.Identity.IsAuthenticated;
}
