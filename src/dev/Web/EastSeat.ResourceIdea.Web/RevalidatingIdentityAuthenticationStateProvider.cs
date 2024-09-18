using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace EastSeat.ResourceIdea.Web;

public class RevalidatingIdentityAuthenticationStateProvider<TUser> : RevalidatingServerAuthenticationStateProvider
    where TUser : class
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IdentityOptions _options;

    public RevalidatingIdentityAuthenticationStateProvider(
        ILoggerFactory loggerFactory,
        IServiceScopeFactory scopeFactory,
        IOptions<IdentityOptions> optionsAccessor)
        : base(loggerFactory)
    {
        _scopeFactory = scopeFactory;
        _options = optionsAccessor.Value;
    }

    protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(30);

    protected override async Task<bool> ValidateAuthenticationStateAsync(AuthenticationState authenticationState, CancellationToken cancellationToken)
    {
        var user = authenticationState.User;
        if (user.Identity is null || user.Identity.IsAuthenticated is false)
        {
            return false;
        }

        var scope = _scopeFactory.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<TUser>>();
        var userId = userManager.GetUserId(user);
        if (userId is null)
        {
            return false;
        }
        var userPrincipal = await userManager.FindByIdAsync(userId);

        return userPrincipal is not null;
    }
}
