using System.Security.Claims;
using System.Text.Json;

using EastSeat.ResourceIdea.Application.Responses;
using EastSeat.ResourceIdea.Persistence.Services;
using EastSeat.ResourceIdea.Web.Services;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace EastSeat.ResourceIdea.Web.Data;

public class ResourceIdeaAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ResourceIdeaAuthenticationService authenticationService;
    private readonly WebUserService webUserService;
    private readonly ProtectedLocalStorage protectedLocalStorage;
    private readonly string webUserStorageKey = "resourceIdeaWebUserIdentity";

    public ApplicationUserViewModel? CurrentUser { get; private set; } = new();

    public ResourceIdeaAuthenticationStateProvider(ResourceIdeaAuthenticationService authenticationService, ProtectedLocalStorage protectedLocalStorage, WebUserService webUserService)
    {
        this.protectedLocalStorage = protectedLocalStorage;
        this.authenticationService = authenticationService;
        this.webUserService = webUserService;
        AuthenticationStateChanged += OnAuthenticationStateChangedAsync;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        ClaimsPrincipal principal = new();
        var user = await webUserService.FetchUserFromBrowserAsync();
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
                await webUserService.PersistUserToBrowserAsync(response.Content);
            }
        }

        return new(principal);
    }

    public async Task<BaseResponse<ApplicationUserViewModel>> LoginAsync(string username, string password)
    {
        ClaimsPrincipal principal = new();
        var response = await webUserService.FindUserFromDatabaseAsync(username, password);
        CurrentUser = response.Content;
        if (response.Success && response.Content is not null)
        {
            principal = response.Content.ToClaimsPrincipal();
        }

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));

        return response;
    }

    public async Task LogoutAsync()
    {
        await ClearBrowserUserDataAsync();
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal())));
    }

    public void Dispose() => AuthenticationStateChanged -= OnAuthenticationStateChangedAsync;

    public async Task ClearBrowserUserDataAsync() => await protectedLocalStorage.DeleteAsync(webUserStorageKey);

    private async void OnAuthenticationStateChangedAsync(Task<AuthenticationState> task)
    {
        var authenticationState = await task;
        if (authenticationState is not null)
        {
            CurrentUser = ApplicationUserViewModel.FromClaimsPrincipal(authenticationState.User);
        }
    }
}
