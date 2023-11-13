using System.Text.Json;

using EastSeat.ResourceIdea.Application.Contracts.Identity;
using EastSeat.ResourceIdea.Application.Models;
using EastSeat.ResourceIdea.Application.Responses;

using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace EastSeat.ResourceIdea.Web.Data;

public class WebUserService
{
    private readonly ProtectedLocalStorage protectedLocalStorage;
    private readonly IAuthenticationService authenticationService;
    private readonly string webUserStorageKey = "resourceIdeaWebUserIdentity";

    public WebUserService(ProtectedLocalStorage protectedLocalStorage, IAuthenticationService authenticationService)
    {
        this.protectedLocalStorage = protectedLocalStorage;
        this.authenticationService = authenticationService;
    }

    public async Task<User?> FindUserFromDatabaseAsync(string username, string password)
    {
        WebAuthenticationResponse authenticationResponse = await authenticationService.AuthenticateWebUserAsync(
            new AuthenticationRequest
            {
                Email = username,
                Password = password
            });

        if (!authenticationResponse.Success)
        {
            return null;
        }

        User subscriptionUser = new()
        {
            Username = authenticationResponse.UserName
        };

        await PersistUserToBrowserAsync(subscriptionUser);

        return subscriptionUser;
    }

    public async Task PersistUserToBrowserAsync(User user)
    {
        string userJson = JsonSerializer.Serialize(user);
        await protectedLocalStorage.SetAsync(webUserStorageKey, userJson);
    }

    public async Task<User?> FetchUserFromBrowserAsync()
    {
        // When Blazor Server is rendering at server side, the browser storage is not available.
        // Therefore, put an empty try/catch block around the call to the browser storage.
        try
        {
            var fetchedUserResult = await protectedLocalStorage.GetAsync<string>(webUserStorageKey);
            if (fetchedUserResult.Success && !string.IsNullOrEmpty(fetchedUserResult.Value))
            {
                string userJson = fetchedUserResult.Value;
                User? user = JsonSerializer.Deserialize<User>(userJson);
                return user;
            }
        }
        catch
        {
        }

        return null;
    }

    public async Task ClearBrowserUserDataAsync() => await protectedLocalStorage.DeleteAsync(webUserStorageKey);
}
