using EastSeat.ResourceIdea.Web.Auth.Models;
using EastSeat.ResourceIdea.Web.Auth.Services;
using MediatR;

namespace EastSeat.ResourceIdea.Web.Auth.Handlers;

public class LogoutCommandHandler(
    IUserAuthenticationService userAuthenticationService) : IRequestHandler<LogoutCommand>
{
    private readonly IUserAuthenticationService _userAuthenticationService = userAuthenticationService;

    public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        await _userAuthenticationService.LogoutAsync();
    }
}