using System.Security.Claims;
using System.Text.Json;

using EastSeat.ResourceIdea.Application.Responses;
using EastSeat.ResourceIdea.Persistence.Services;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace EastSeat.ResourceIdea.Web.Data;

public class ResourceIdeaAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly AuthenticationService authenticationService;

    private readonly ProtectedLocalStorage protectedLocalStorage;
    private readonly string webUserStorageKey = "resourceIdeaWebUserIdentity";

    public ApplicationUserViewModel? CurrentUser { get; private set; } = new();

    public ResourceIdeaAuthenticationStateProvider(AuthenticationService authenticationService, ProtectedLocalStorage protectedLocalStorage)
    {
        this.protectedLocalStorage = protectedLocalStorage;
        this.authenticationService = authenticationService;
        AuthenticationStateChanged += OnAuthenticationStateChangedAsync;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        ClaimsPrincipal principal = new();
        var user = await FetchUserFromBrowserAsync();
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
                await PersistUserToBrowserAsync(response.Content);
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
            await PersistUserToBrowserAsync(response.Content);
        }

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));

        return response;
    }

    public async Task LogoutAsync()
    {
        await ClearBrowserUserDataAsync();
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal())));
    }

    public async Task PersistUserToBrowserAsync(ApplicationUserViewModel user)
    {
        string userJson = JsonSerializer.Serialize(user);
        await protectedLocalStorage.SetAsync(webUserStorageKey, userJson);
    }

    public async Task<ApplicationUserViewModel?> FetchUserFromBrowserAsync()
    {
        // When Blazor Server is rendering at server side, the browser storage is not available.
        // Therefore, put an empty try/catch block around the call to the browser storage.
        try
        {
            var fetchedUserResult = await protectedLocalStorage.GetAsync<string>(webUserStorageKey);
            if (fetchedUserResult.Success && !string.IsNullOrEmpty(fetchedUserResult.Value))
            {
                string userJson = fetchedUserResult.Value;
                ApplicationUserViewModel? user = JsonSerializer.Deserialize<ApplicationUserViewModel>(userJson);
                return user;
            }
        }
        catch
        {
        }

        return null;
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
