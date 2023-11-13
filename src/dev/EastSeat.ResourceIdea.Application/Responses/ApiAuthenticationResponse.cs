namespace EastSeat.ResourceIdea.Application.Responses;

public class ApiAuthenticationResponse : AuthenticationResponse
{
    /// <inheritdoc />
    public string Token { get; set; } = string.Empty;
}
