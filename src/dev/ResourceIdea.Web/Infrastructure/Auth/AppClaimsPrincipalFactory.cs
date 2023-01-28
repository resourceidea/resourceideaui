using System.Security.Claims;
using Microsoft.Extensions.Options;

namespace ResourceIdea.Infrastructure.Auth;

public class AppClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
{
    public AppClaimsPrincipalFactory(
        UserManager<ApplicationUser> userManager, 
        RoleManager<IdentityRole> roleManager, 
        IOptions<IdentityOptions> options) : base(userManager, roleManager, options)
    {
    }

    public async override Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
    {
        var principal = await base.CreateAsync(user);
        var identity = principal.Identity as ClaimsIdentity;
        if (identity is not null)
        {
            identity.AddClaims(new[] { new Claim(ClaimTypes.NameIdentifier, user.Id)});
        }

        return principal;
    }
}