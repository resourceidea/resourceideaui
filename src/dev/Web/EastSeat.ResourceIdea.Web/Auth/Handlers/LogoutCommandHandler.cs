using EastSeat.ResourceIdea.Web.Auth.Models;
using EastSeat.ResourceIdea.Web.Auth.Services;
using MediatR;

namespace EastSeat.ResourceIdea.Web.Auth.Handlers;

public class LogoutCommandHandler(
    IUserAuthenticationService userAuthenticationService) : IRequestHandler<LogoutCommand, bool>
{
    private readonly IUserAuthenticationService _userAuthenticationService = userAuthenticationService;

    public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
		try
		{
			await _userAuthenticationService.LogoutAsync();
			return true;
		}
		catch
		{
			return false;
		}
    }
}