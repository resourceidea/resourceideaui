using System.Text.Json;

using AutoMapper;

using EastSeat.ResourceIdea.Application.Contracts.Identity;
using EastSeat.ResourceIdea.Application.Responses;
using EastSeat.ResourceIdea.Persistence.Models;

using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Identity;

namespace EastSeat.ResourceIdea.Web.Services;

/// <summary>
/// Web authentication service.
/// </summary>
public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly SignInManager<ApplicationUser> signInManager;
    private readonly IMapper mapper;

    private readonly ProtectedLocalStorage protectedLocalStorage;
    private readonly string webUserStorageKey = "resourceIdeaWebUserIdentity";

    public AuthenticationService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IMapper mapper,
        ProtectedLocalStorage protectedLocalStorage)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.mapper = mapper;
        this.protectedLocalStorage = protectedLocalStorage;
    }

    public async Task<BaseResponse<ApplicationUserViewModel>> AuthenticateUserAsync(AuthenticationRequest request)
    {
        var applicationUser = await userManager.FindByEmailAsync(request.Email);
        if (applicationUser is null)
        {
            return GetFailedResponse(message: "User not found", errorCode: "UserNotFound");
        }

        var result = await signInManager.PasswordSignInAsync(applicationUser.UserName ?? string.Empty, request.Password, false, false);
        if (!result.Succeeded)
        {
            return GetFailedResponse(message: "Invalid credentials", errorCode: "InvalidCredentials");
        }

        var response = new BaseResponse<ApplicationUserViewModel>
        {
            Success = true,
            Content = mapper.Map<ApplicationUserViewModel>(applicationUser)
        };
        await PersistUserToBrowserAsync(response.Content);

        return response;
    }

    private static BaseResponse<ApplicationUserViewModel> GetFailedResponse(string message, string errorCode)
    {
        return new BaseResponse<ApplicationUserViewModel>
        {
            Success = false,
            Content = null,
            Message = message,
            ErrorCode = errorCode
        };
    }

    public Task DeleteUserAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<BaseResponse<ApplicationUserViewModel>> GetApplicationUserAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<UserRegistrationResponse> RegisterUserAsync(UserRegistrationRequest request)
    {
        throw new NotImplementedException();
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

    public async Task ClearBrowserUserDataAsync() => await protectedLocalStorage.DeleteAsync(webUserStorageKey);
}
