using System.Security.Claims;

using EastSeat.ResourceIdea.Application.Responses;
using EastSeat.ResourceIdea.Web.Services;

using Microsoft.AspNetCore.Components.Authorization;

namespace EastSeat.ResourceIdea.Web.Data;

public class ResourceIdeaAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly AuthenticationService authenticationService;

    public ApplicationUserViewModel? CurrentUser { get; private set; } = new();

    public ResourceIdeaAuthenticationStateProvider(AuthenticationService authenticationService)
    {
        this.authenticationService = authenticationService;
        AuthenticationStateChanged += OnAuthenticationStateChangedAsync;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        ClaimsPrincipal principal = new();
        var user = await authenticationService.FetchUserFromBrowserAsync();
        if (user is not null)
        {
            var response = await authenticationService.AuthenticateUserAsync(
                new AuthenticationRequest
                {
                    Email = user.UserName ?? string.Empty
                });

            CurrentUser = response.Content;

            if (response.Success && response.Content is not null)
            {
                principal = response.Content.ToClaimsPrincipal();
            }
        }

        return new(principal);
    }

    public async Task<BaseResponse<ApplicationUserViewModel>> LoginAsync(string username, string password)
    {
        ClaimsPrincipal principal = new();
        var response = await authenticationService.AuthenticateUserAsync(
            new AuthenticationRequest
            {
                Email = username,
                Password = password
            });
        if (response.Success && response.Content is not null)
        {
            principal = response.Content.ToClaimsPrincipal();
            CurrentUser = response.Content;
        }

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));

        return response;
    }

    public async Task LogoutAsync()
    {
        await authenticationService.ClearBrowserUserDataAsync();

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal())));
    }

    private async void OnAuthenticationStateChangedAsync(Task<AuthenticationState> task)
    {
        var authenticationState = await task;
        if (authenticationState is not null)
        {
            CurrentUser = ApplicationUserViewModel.FromClaimsPrincipal(authenticationState.User);
        }
    }

    public void Dispose() => AuthenticationStateChanged -= OnAuthenticationStateChangedAsync;
}
