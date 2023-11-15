using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using EastSeat.ResourceIdea.Application.Contracts.Identity;
using EastSeat.ResourceIdea.Application.Features.ApplicationUser.Commands.CreateApplicationUser;
using EastSeat.ResourceIdea.Application.Models;
using EastSeat.ResourceIdea.Application.Responses;
using EastSeat.ResourceIdea.Persistence.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EastSeat.ResourceIdea.Persistence.Services;

/// <summary>
/// Implements <see cref="IAuthenticationService"/>.
/// </summary>
public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly JwtSettings _jwtSettings;

    /// <summary>
    /// Initializes an instance of <see cref="AuthenticationService"/>.
    /// </summary>
    /// <param name="userManager"></param>
    /// <param name="jwtSettings"></param>
    /// <param name="signInManager"></param>
    public AuthenticationService(
        UserManager<ApplicationUser> userManager,
        IOptions<JwtSettings> jwtSettings,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _jwtSettings = jwtSettings.Value;
        _signInManager = signInManager;
    }

    /// <inheritdoc />
    //public async Task<BaseResponse<ApplicationUserViewModel>> AuthenticateApiUserAsync(AuthenticationRequest request)
    //{
    //    var user = await _userManager.FindByEmailAsync(request.Email) ?? throw new Exception($"User with {request.Email} not found.");
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

    //    var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);

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
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return new BaseResponse<ApplicationUserViewModel>
            {
                Success = false,
                Content = null,
                Message = "User not found",
                ErrorCode = "UserNotFound",
                Errors = new List<string> { "User not found" }
            };
        }

        return new BaseResponse<ApplicationUserViewModel>
        {
            Success = true,
            Content = new ApplicationUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                SubscriptionId = user.SubscriptionId
            }
        };
    }

    /// <inheritdoc />
    public async Task DeleteUserAsync(Guid userId)
    {
        var applicationUser = await _userManager.FindByIdAsync(userId.ToString());
        if (applicationUser is not null)
        {
            await _userManager.DeleteAsync(applicationUser);
        }
    }

    /// <inheritdoc />
    public async Task<UserRegistrationResponse> RegisterUserAsync(UserRegistrationRequest request)
    {
        var existingUser = await _userManager.FindByNameAsync(request.Email);

        if (existingUser != null)
        {
            return new UserRegistrationResponse
            {
                Success = false,
                Message = $"Username already exists."
            };
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

        UserRegistrationResponse response = new();
        var existingEmail = await _userManager.FindByEmailAsync(request.Email);
        if (existingEmail == null)
        {
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                response.Content = new CreateApplicationUserViewModel
                {
                    UserId = Guid.Parse(user.Id),
                    SubscriptionId = user.SubscriptionId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email
                };
            }
            else
            {
                response.Success = false;
                response.Message = Constants.ErrorMessages.Commands.CreateApplicationUsers.UserRegistrationFailed;
                response.Errors = new List<string>();
                foreach (var error in result.Errors)
                {
                    response.Errors.Add(error.Description);
                }
            }
        }
        else
        {
            response.Success = false;
            response.Message = Constants.ErrorMessages.Commands.CreateApplicationUsers.EmailExists;
        }

        return response;
    }

    /// <inheritdoc />
    public async Task<BaseResponse<ApplicationUserViewModel>> GetApplicationUserAsync(Guid id)
    {
        var applicationUser = await _userManager.FindByIdAsync(id.ToString());
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

    private async Task<JwtSecurityToken> GenerateToken(ApplicationUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        var roleClaims = new List<Claim>();

        for (int i = 0; i < roles.Count; i++)
        {
            roleClaims.Add(new Claim("roles", roles[i]));
        }

        var claims = new[]
        {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim("uid", user.Id)
            }
        .Union(userClaims)
        .Union(roleClaims);

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
            signingCredentials: signingCredentials);
        return jwtSecurityToken;
    }
}
