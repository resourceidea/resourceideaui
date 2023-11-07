using EastSeat.ResourceIdea.Application.Contracts.Identity;

using FluentValidation.Results;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.ApplicationUser.Commands.CreateApplicationUser;

public class CreateApplicationUserCommandHandler : IRequestHandler<CreateApplicationUserCommand, CreateApplicationUserCommandResponse>
{
    private readonly IAuthenticationService authenticationService;

    public CreateApplicationUserCommandHandler(IAuthenticationService authenticationService)
    {
        this.authenticationService = authenticationService;
    }

    public async Task<CreateApplicationUserCommandResponse> Handle(CreateApplicationUserCommand request, CancellationToken cancellationToken)
    {
        CreateApplicationUserCommandResponse response = new ();

        CreateApplicationUserCommandValidator commandValidator = new ();
        ValidationResult commandValidationResult = await commandValidator.ValidateAsync(request, cancellationToken);
        if (commandValidationResult.Errors.Count > 0)
        {
            response.Success = false;
            response.Errors = new List<string>();
            foreach (var error in commandValidationResult.Errors)
            {
                response.Errors.Add(error.ErrorMessage);
            }
        }

        if (response.Success)
        {
            var userRegistrationResponse = await authenticationService.RegisterUserAsync(new Models.UserRegistrationRequest
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Password = request.Password,
            });

            response.Success = userRegistrationResponse.Success;
            response.Message = userRegistrationResponse.Message;
            response.Errors = userRegistrationResponse.Errors;
            response.ApplicationUser = userRegistrationResponse.ApplicationUser;
        }

        return response;
    }
}
