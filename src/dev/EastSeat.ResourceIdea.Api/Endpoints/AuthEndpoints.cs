using EastSeat.ResourceIdea.Api.Endpoints.EndpointHandlers;

namespace EastSeat.ResourceIdea.Api.AppRoutes;

/// <summary>
/// Endpoints that handle authentication and authorization requests.
/// </summary>
public static class AuthEndpoints
{
    /// <summary>
    /// Maps authentication and authorization endpoints.
    /// </summary>
    /// <param name="app"></param>
    public static void MapAuthEndpoints(this WebApplication app)
    {
        app.MapPost($"{Constants.ApiBaseRoutes.Authenticate}", AuthEndpointHandlers.AuthenticateAsync);
    }
}
