using System.Data;
using System.Security.Claims;

using Microsoft.AspNetCore.Components.Authorization;

namespace EastSeat.ResourceIdea.Web.Data;

public class ResourceIdeaAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly WebUserService webUserService;

    public User? CurrentUser { get; private set; } = new ();

    public ResourceIdeaAuthenticationStateProvider(WebUserService webUserService)
    {
        this.webUserService = webUserService;
        AuthenticationStateChanged += OnAuthenticationStateChangedAsync;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        ClaimsPrincipal principal = new ();
        var user = await webUserService.FetchUserFromBrowserAsync();
        if (user is not null)
        {
            var authenticatedUser = await webUserService.FindUserFromDatabaseAsync(user.Username, user.Password);
            CurrentUser = authenticatedUser;

            if (authenticatedUser is not null)
            {
                principal = authenticatedUser.ToClaimsPrincipal();
            }
        }

        return new(principal);
    }

    public async Task LoginAsync(string username, string password)
    {
        ClaimsPrincipal principal = new ();
        User? user = await webUserService.FindUserFromDatabaseAsync(username, password);
        if (user is not null)
        {
            principal = user.ToClaimsPrincipal();
            CurrentUser = user;
        }

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
    }

    public async Task LogoutAsync()
    {
        await webUserService.ClearBrowserUserDataAsync();

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal())));
    }

    private async void OnAuthenticationStateChangedAsync(Task<AuthenticationState> task)
    {
        var authenticationState = await task;
        if (authenticationState is not null)
        {
            CurrentUser = User.FromClaimsPrincipal(authenticationState.User);
        }
    }

    public void Dispose() => AuthenticationStateChanged -= OnAuthenticationStateChangedAsync;
}
