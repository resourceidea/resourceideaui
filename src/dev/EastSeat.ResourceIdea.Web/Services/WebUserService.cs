using System.Text.Json;

using EastSeat.ResourceIdea.Application.Contracts.Identity;
using EastSeat.ResourceIdea.Application.Responses;
using EastSeat.ResourceIdea.Persistence.Services;

using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace EastSeat.ResourceIdea.Web.Services;

public class WebUserService
{
    private readonly ProtectedLocalStorage protectedLocalStorage;
    private readonly string webUserStorageKey = "resourceIdeaWebUserIdentity";
    private readonly IResourceIdeaAuthenticationService authenticationService;

    public WebUserService(ProtectedLocalStorage protectedLocalStorage, IResourceIdeaAuthenticationService authenticationService)
    {
        this.protectedLocalStorage = protectedLocalStorage;
        this.authenticationService = authenticationService;
    }

    public async Task<BaseResponse<ApplicationUserViewModel>> FindUserFromDatabaseAsync(string username, string password)
    {
        return await authenticationService.AuthenticateUserAsync(new AuthenticationRequest
        {
            Email = username,
            Password = password
        });
    }

    public Task<BaseResponse<ApplicationUserViewModel>> FindUserFromDatabaseAsync(string token)
    {
        throw new NotImplementedException();
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

    public async Task PersistUserToBrowserAsync(ApplicationUserViewModel user)
    {
        string userJson = JsonSerializer.Serialize(user);
        await protectedLocalStorage.SetAsync(webUserStorageKey, userJson);
    }

    public Task IsExpiredToken(string token)
    {
        throw new NotImplementedException();
    }

    public Task GetNewToken(string refreshToken)
    {
        throw new NotImplementedException();
    }
}
