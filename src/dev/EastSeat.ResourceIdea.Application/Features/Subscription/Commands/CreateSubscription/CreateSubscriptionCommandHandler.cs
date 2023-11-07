using AutoMapper;

using EastSeat.ResourceIdea.Application.Contracts.Identity;
using EastSeat.ResourceIdea.Application.Contracts.Persistence;
using EastSeat.ResourceIdea.Application.Features.ApplicationUser.Commands.CreateApplicationUser;
using EastSeat.ResourceIdea.Application.Features.Asset.Commands;
using EastSeat.ResourceIdea.Application.Features.Employee.Commands.CreateEmployee;
using EastSeat.ResourceIdea.Application.Models;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Subscription.Commands.CreateSubscription;

/// <summary>
/// Handles the create subscription command.
/// </summary>
public class CreateSubscriptionCommandHandler : IRequestHandler<CreateSubscriptionCommand, CreateSubscriptionCommandResponse>
{
    private readonly IMapper mapper;
    private readonly ISubscriptionRepository subscriptionRepository;
    private readonly IAuthenticationService authenticationService;
    private readonly IEmployeeRepository employeeRepository;

    /// <summary>
    /// Initializes <see cref="CreateSubscriptionCommandHandler"/>.
    /// </summary>
    /// <param name="mapper">Mapper.</param>
    /// <param name="subscriptionRepository">Subscription repository.</param>
    /// <param name="authentication">Authentication service.</param>
    public CreateSubscriptionCommandHandler(
        IMapper mapper,
        ISubscriptionRepository subscriptionRepository,
        IAuthenticationService authenticationService,
        IEmployeeRepository employeeRepository)
    {
        this.mapper = mapper;
        this.subscriptionRepository = subscriptionRepository;
        this.authenticationService = authenticationService;
        this.employeeRepository = employeeRepository;
    }

    /// <inheritdoc />
    public async Task<CreateSubscriptionCommandResponse> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var response = new CreateSubscriptionCommandResponse();

        // Validate the command.
        var commandValidator = new CreateSubscriptionCommandValidator();
        var commandValidationResult = await commandValidator.ValidateAsync(request, cancellationToken);

        if (commandValidationResult.Errors.Count > 0)
        {
            response.Success = false;
            response.Errors = [];
            foreach (var error in commandValidationResult.Errors)
            {
                response.Errors.Add(error.ErrorMessage);
            }
        }

        await CreateSubscriptionAsync(request, response);

        await CreateApplicationUserAsync(request, response);

        await CreateEmployeeAsync(request, response);

        if (response.Success is false)
        {
            response = await RollbackCreateSubscriptionCommandAsync(response);
        }

        return response;
    }

    private async Task<CreateSubscriptionCommandResponse> RollbackCreateSubscriptionCommandAsync(CreateSubscriptionCommandResponse response)
    {
        if (response.Subscription is not null)
        {
            await subscriptionRepository.DeleteAsync(response.Subscription.SubscriptionId);
        }

        if (response.ApplicationUser is not null)
        {
            await authenticationService.DeleteUserAsync(response.ApplicationUser.UserId);
        }

        if (response.Employee is not null)
        {
            await employeeRepository.DeleteAsync(Guid.Parse(response.Employee.UserId));
        }

        response.Subscription = default!;
        response.ApplicationUser = default!;
        response.Employee = default!;

        return response;
    }

    private async Task CreateSubscriptionAsync(CreateSubscriptionCommand request, CreateSubscriptionCommandResponse response)
    {
        if (await subscriptionRepository.IsSubscriberNameAlreadyInUse(request.SubscriberName))
        {
            response.Success = false;
            response.Message = Constants.ErrorMessages.Commands.CreateSubscriptions.SubscriberNameAlreadyInUse;
            response.ErrorCode = nameof(Constants.ErrorMessages.Commands.CreateSubscriptions.SubscriberNameAlreadyInUse);

            return;
        }

        var subscription = new Domain.Entities.Subscription
        {
            StartDate = request.StartDate,
            SubscriberName = request.SubscriberName,
            SubscriptionId = request.SubscriptionId,
            Status = request.Status
        };

        subscription = await subscriptionRepository.AddAsync(subscription);
        response.Subscription = mapper.Map<CreateSubscriptionViewModel>(subscription);
    }

    private async Task CreateApplicationUserAsync(CreateSubscriptionCommand request, CreateSubscriptionCommandResponse response)
    {
        if (response.Success is false)
        {
            return;
        }

        var userRegistrationResponse = await authenticationService.RegisterUserAsync(new UserRegistrationRequest
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Password = request.Password,
            SubscriptionId = response.Subscription.SubscriptionId
        });

        response.ApplicationUser = userRegistrationResponse.ApplicationUser;
        response.Success = userRegistrationResponse.Success;
        response.Message = userRegistrationResponse.Message;
        response.Errors = userRegistrationResponse.Errors;
    }

    private async Task CreateEmployeeAsync(CreateSubscriptionCommand request, CreateSubscriptionCommandResponse response)
    {
        if (response.Success is false)
        {
            return;
        }

        if (UserRegistrationFailed(response))
        {
            response.Success = false;
            response.Message = $"User registration failed.";

            return;
        }

        if (await employeeRepository.IsExisitingEmployee(request.SubscriptionId, response.ApplicationUser.UserId.ToString()))
        {
            response.Success = false;
            response.Message = $"User is already an employee of the subscription.";
        }
        else
        {
            var employee = new Domain.Entities.Employee
            {
                SubscriptionId = request.SubscriptionId,
                JobPositionId = request.JobPositionId == Guid.Empty ? null : request.JobPositionId,
                UserId = response.ApplicationUser.UserId.ToString(),
                Id = Guid.NewGuid()
            };
            employee = await employeeRepository.AddAsync(employee);
            response.Employee = mapper.Map<CreateEmployeeViewModel>(employee);
        }
    }

    private static bool UserRegistrationFailed(CreateSubscriptionCommandResponse response)
    {
        return response.ApplicationUser == default || response.ApplicationUser.FirstName == string.Empty || response.ApplicationUser.UserId.Equals(Guid.Empty);
    }
}
