using AutoMapper;

using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Users.Models;
using EastSeat.ResourceIdea.Web.Auth.Models;
using EastSeat.ResourceIdea.Web.Auth.Services;

using MediatR;

namespace EastSeat.ResourceIdea.Web.Auth.Handlers;

public sealed class LoginCommandHandler(
    IUserAuthenticationService userAuthenticationService,
    IMapper mapper) : IRequestHandler<LoginCommand, ResourceIdeaResponse<LoginModel>>
{
    private readonly IUserAuthenticationService _userAuthenticationService = userAuthenticationService;
    private readonly IMapper _mapper = mapper;

    public async Task<ResourceIdeaResponse<LoginModel>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var loginModel = _mapper.Map<LoginModel>(request);
        return await _userAuthenticationService.LoginAsync(loginModel, cancellationToken);
    }
}
