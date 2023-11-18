using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using AutoMapper;

using EastSeat.ResourceIdea.Application.Contracts.Identity;
using EastSeat.ResourceIdea.Application.Features.ApplicationUser.Commands.CreateApplicationUser;
using EastSeat.ResourceIdea.Application.Models;
using EastSeat.ResourceIdea.Application.Responses;
using EastSeat.ResourceIdea.Persistence.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EastSeat.ResourceIdea.Persistence.Services;

/// <summary>
/// Implements <see cref="IResourceIdeaAuthenticationService"/>.
/// </summary>
public class ResourceIdeaAuthenticationService : IResourceIdeaAuthenticationService
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly SignInManager<ApplicationUser> signInManager;
    private readonly JwtSettings jwtSettings;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes an instance of <see cref="ResourceIdeaAuthenticationService"/>.
    /// </summary>
    /// <param name="userManager"></param>
    /// <param name="jwtSettings"></param>
    /// <param name="signInManager"></param>
    public ResourceIdeaAuthenticationService(
        UserManager<ApplicationUser> userManager,
        IOptions<JwtSettings> jwtSettings,
        SignInManager<ApplicationUser> signInManager,
        IMapper mapper)
    {
        this.userManager = userManager;
        this.jwtSettings = jwtSettings.Value;
        this.signInManager = signInManager;
        this.mapper = mapper;
    }

    // Uncomment block when implementing API authentication.
    /// <inheritdoc />
    //public async Task<BaseResponse<ApplicationUserViewModel>> AuthenticateApiUserAsync(AuthenticationRequest request)
    //{
    //    var user = await userManager.FindByEmailAsync(request.Email) ?? throw new Exception($"User with {request.Email} not found.");
    //    if (user.UserName is null || user.Email is null)
    //    {
    //        return new BaseResponse<ApplicationUserViewModel>
    //        {
    //            Success = false,
    //            Message = "User not found",
    //            Content = null,
    //            Errors = new List<string> { "User not found"},
    //            ErrorCode = "UserNotFound"
    //        };
    //    }

    //    var result = await signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);

    //    if (!result.Succeeded)
    //    {
    //        return new BaseResponse<ApplicationUserViewModel>
    //        {
    //            Success = false,
    //            Message = "Invalid user credentials",
    //            Content = null,
    //            Errors = new List<string> { "Invalid user credentials" },
    //            ErrorCode = "InvalidUserCredentials"
    //        };
    //    }

    //    JwtSecurityToken jwtSecurityToken = await GenerateToken(user);

    //    BaseResponse<ApplicationUserViewModel> response = new()
    //    {
    //        Success = result.Succeeded,
    //        Content = new ApplicationUserViewModel
    //        {
    //            Email = user.
    //        }
    //        Id = user.Id,
    //        Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
    //        Email = user.Email,
    //        UserName = user.UserName
    //    };

    //    return response;
    //}

    /// <inheritdoc />
    public async Task<BaseResponse<ApplicationUserViewModel>> AuthenticateUserAsync(AuthenticationRequest request)
    {
        var applicationUser = await userManager.FindByEmailAsync(request.Email);
        if (applicationUser is null)
        {
            return GetFailedResponse(message: "User not found", errorCode: "UserNotFound");
        }

        var result = await signInManager.CheckPasswordSignInAsync(applicationUser, request.Password, false);
        if (!result.Succeeded)
        {
            return GetFailedResponse(message: "Invalid login credentials entered", errorCode: "InvalidCredentials");
        }

        var response = new BaseResponse<ApplicationUserViewModel>
        {
            Success = true,
            Content = mapper.Map<ApplicationUserViewModel>(applicationUser)
        };

        return response;
    }

    /// <inheritdoc />
    public async Task DeleteUserAsync(Guid userId)
    {
        var applicationUser = await userManager.FindByIdAsync(userId.ToString());
        if (applicationUser is not null)
        {
            await userManager.DeleteAsync(applicationUser);
        }
    }

    /// <inheritdoc />
    public async Task<BaseResponse<CreateApplicationUserViewModel>> RegisterUserAsync(UserRegistrationRequest request)
    {
        BaseResponse<CreateApplicationUserViewModel> response = new();

        if (await UserExistsAsync(request))
        {
            response.Success = false;
            response.Message = Constants.ErrorMessages.Commands.CreateApplicationUsers.EmailExists;

            return response;
        }

        var user = new ApplicationUser
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.Email,
            EmailConfirmed = true,
            SubscriptionId = request.SubscriptionId
        };

        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            response.Success = false;
            response.Message = Constants.ErrorMessages.Commands.CreateApplicationUsers.UserRegistrationFailed;
            response.Errors = new List<string>();
            foreach (var error in result.Errors)
            {
                response.Errors.Add(error.Description);
            }

            return response;
        }

        response.Content = new CreateApplicationUserViewModel
        {
            UserId = Guid.Parse(user.Id),
            SubscriptionId = user.SubscriptionId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };

        return response;
    }

    /// <inheritdoc />
    public async Task<BaseResponse<ApplicationUserViewModel>> GetApplicationUserAsync(Guid id)
    {
        var applicationUser = await userManager.FindByIdAsync(id.ToString());
        if (applicationUser is null)
        {
            return new BaseResponse<ApplicationUserViewModel>
            {
                Success = false,
                Content = null,
                Message = $"Application user with Id {id} not found.",
                ErrorCode = "ApplicationUserNotFound",
                Errors = new List<string>() { $"Application user with Id {id} not found." }
            };
        }

        return new BaseResponse<ApplicationUserViewModel>
        {
            Success = true,
            Content = new ApplicationUserViewModel
            {
                Id = applicationUser.Id,
                UserName = applicationUser.UserName,
                FirstName = applicationUser.FirstName,
                SubscriptionId = applicationUser.SubscriptionId
            }
        };
    }

    // Uncomment block when implementing API authentication.
    //private async Task<JwtSecurityToken> GenerateToken(ApplicationUser user)
    //{
    //    var userClaims = await userManager.GetClaimsAsync(user);
    //    var roles = await userManager.GetRolesAsync(user);

    //    var roleClaims = new List<Claim>();

    //    for (int i = 0; i < roles.Count; i++)
    //    {
    //        roleClaims.Add(new Claim("roles", roles[i]));
    //    }

    //    var claims = new[]
    //    {
    //            new Claim(JwtRegisteredClaimNames.Sub, user.UserName ?? string.Empty),
    //            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    //            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
    //            new Claim("uid", user.Id)
    //        }
    //    .Union(userClaims)
    //    .Union(roleClaims);

    //    var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
    //    var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

    //    var jwtSecurityToken = new JwtSecurityToken(
    //        issuer: jwtSettings.Issuer,
    //        audience: jwtSettings.Audience,
    //        claims: claims,
    //        expires: DateTime.UtcNow.AddMinutes(jwtSettings.DurationInMinutes),
    //        signingCredentials: signingCredentials);
    //    return jwtSecurityToken;
    //}

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

    private async Task<bool> UserExistsAsync(UserRegistrationRequest request)
    {
        var existingUser = await userManager.FindByEmailAsync(request.Email);
        var existingEmail = await userManager.FindByNameAsync(request.Email);

        if (existingUser is not null || existingEmail is not null)
        {
            return true;
        }

        return false;
    }
}
