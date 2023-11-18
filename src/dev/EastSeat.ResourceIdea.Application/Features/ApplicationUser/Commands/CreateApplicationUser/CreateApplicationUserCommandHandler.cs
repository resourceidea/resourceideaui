using EastSeat.ResourceIdea.Application.Contracts.Identity;
using EastSeat.ResourceIdea.Application.Models;
using EastSeat.ResourceIdea.Application.Responses;

using FluentValidation.Results;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.ApplicationUser.Commands.CreateApplicationUser;

public class CreateApplicationUserCommandHandler : IRequestHandler<CreateApplicationUserCommand, BaseResponse<CreateApplicationUserViewModel>>
{
    private readonly IResourceIdeaAuthenticationService authenticationService;

    public CreateApplicationUserCommandHandler(IResourceIdeaAuthenticationService authenticationService)
    {
        this.authenticationService = authenticationService;
    }

    public async Task<BaseResponse<CreateApplicationUserViewModel>> Handle(CreateApplicationUserCommand request, CancellationToken cancellationToken)
    {
        BaseResponse<CreateApplicationUserViewModel> response = new ();

        CreateApplicationUserCommandValidator commandValidator = new ();
        ValidationResult commandValidationResult = await commandValidator.ValidateAsync(request, cancellationToken);
        response.Success = commandValidationResult.IsValid;
        if (commandValidationResult.Errors.Count > 0)
        {
            response.Errors = new List<string>();
            foreach (var error in commandValidationResult.Errors)
            {
                response.Errors.Add(error.ErrorMessage);
            }
        }

        if (response.Success)
        {
            response = await authenticationService.RegisterUserAsync(new UserRegistrationRequest
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Password = request.Password,
                SubscriptionId = request.SubscriptionId
            });
        }

        return response;
    }
}
