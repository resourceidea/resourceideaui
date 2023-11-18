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
    public static Task<IResult> AuthenticateAsync(IResourceIdeaAuthenticationService authenticationService, AuthenticationRequest authenticationRequest)
    {
        //var authenticationResponse = await authenticationService.AuthenticateApiUserAsync(authenticationRequest);
        //var response = new BaseResponse<AuthenticationResponse>
        //{
        //    Content = authenticationResponse.
        //}; (
        //    authenticationResponse,
        //    authenticationResponse.Success,
        //    authenticationResponse.Message,
        //    authenticationResponse.ErrorCode
        //);

        //return response.Success ? TypedResults.Ok(response) : TypedResults.BadRequest(response);
        throw new NotImplementedException();
    }
}
