using EastSeat.ResourceIdea.Application.Contracts.Identity;
using EastSeat.ResourceIdea.Application.Models;
using EastSeat.ResourceIdea.Application.Responses;

namespace EastSeat.ResourceIdea.Api.Endpoints.EndpointHandlers;

/// <summary>
/// Handlers for the authentication and authorization endpoints.
/// </summary>
public static class AuthEndpointHandlers
{
    /// <summary>
    /// Handles authentication requests.
    /// </summary>
    /// <param name="authenticationService"></param>
    /// <param name="authenticationRequest"></param>
    /// <returns></returns>
    public static async Task<IResult> AuthenticateAsync(IAuthenticationService authenticationService, AuthenticationRequest authenticationRequest)
    {
        var authenticationResponse = await authenticationService.AuthenticateApiUserAsync(authenticationRequest);
        var response = new ApiResponse<AuthenticationResponse>(
            authenticationResponse,
            authenticationResponse.Success,
            authenticationResponse.Message,
            authenticationResponse.ErrorCode
        );

        return response.Success ? TypedResults.Ok(response) : TypedResults.BadRequest(response);
    }
}
