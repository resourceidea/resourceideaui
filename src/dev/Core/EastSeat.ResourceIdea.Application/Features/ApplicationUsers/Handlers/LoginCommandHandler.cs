using AutoMapper;
using EastSeat.ResourceIdea.Application.Enums;
using EastSeat.ResourceIdea.Application.Features.ApplicationUsers.Commands;
using EastSeat.ResourceIdea.Application.Features.ApplicationUsers.Contracts;
using EastSeat.ResourceIdea.Application.Features.ApplicationUsers.Validators;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Users.Models;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.ApplicationUsers.Handlers;

public class LoginCommandHandler(IUserAuthenticationService userAuthenticationService, IMapper mapper)
    : IRequestHandler<LoginCommand, ResourceIdeaResponse<LoginModel>>
{
    private readonly IUserAuthenticationService _userAuthenticationService = userAuthenticationService;
    private readonly IMapper _mapper = mapper;

    public async Task<ResourceIdeaResponse<LoginModel>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var commandValidationResponse = await ValidateCommand(request, cancellationToken);
        if (commandValidationResponse.IsFailure)
        {
            return commandValidationResponse;
        }

        LoginModel loginRequest = _mapper.Map<LoginModel>(request);
        return await _userAuthenticationService.LoginAsync(loginRequest, cancellationToken);
    }

    private static async Task<ResourceIdeaResponse<LoginModel>> ValidateCommand(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var validationResponse = ResourceIdeaResponse<LoginModel>.Success(Optional<LoginModel>.None);
        
        LoginCommandValidator validator = new();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid is false || validationResult.Errors.Count > 0)
        {
            validationResponse = ResourceIdeaResponse<LoginModel>.Failure(ErrorCode.LoginCommandValidationFailure);
        }

        return validationResponse;
    }
}
