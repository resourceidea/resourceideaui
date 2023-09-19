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

        if (response.Success)
        {
            await CreateSubscriptionAsync(request, response);

            UserRegistrationResponse userRegistrationResponse = await CreateApplicationUserAsync(request, response);

            if (userRegistrationResponse.Success)
            {
                await CreateEmployeeAsync(request, response, userRegistrationResponse.ApplicationUser.UserId.ToString()); 
            }
            else
            {
                response.Success = userRegistrationResponse.Success;
                response.Message = userRegistrationResponse.Message;
            }
        }

        return response;
    }

    private async Task CreateSubscriptionAsync(CreateSubscriptionCommand request, CreateSubscriptionCommandResponse response)
    {
        if (await subscriptionRepository.IsSubscriberNameAlreadyInUse(request.SubscriberName))
        {
            response.Success = false;
            response.Message = Constants.ErrorMessages.Commands.CreateSubscriptions.SubscriberNameAlreadyInUse;

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

    private async Task<UserRegistrationResponse> CreateApplicationUserAsync(CreateSubscriptionCommand request, CreateSubscriptionCommandResponse response)
    {
        if (response.Success is false)
        {
            return new UserRegistrationResponse(response.Success, response.Message);
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

        return userRegistrationResponse;
    }

    private async Task CreateEmployeeAsync(CreateSubscriptionCommand request, CreateSubscriptionCommandResponse response, string userId)
    {
        if (response.Success is false)
        {
            return;
        }

        if (await employeeRepository.IsExisitingEmployee(request.SubscriptionId, userId))
        {
            response.Success = false;
            response.Message = $"User {userId} is already an employee of the subscription {request.SubscriptionId}.";
        }
        else
        {
            var employee = new Domain.Entities.Employee
            {
                SubscriptionId = request.SubscriptionId,
                JobPositionId = request.JobPositionId == Guid.Empty ? null : request.JobPositionId,
                UserId = userId,
                Id = Guid.NewGuid()
            };
            employee = await employeeRepository.AddAsync(employee);
            response.Employee = mapper.Map<CreateEmployeeViewModel>(employee);
        }
    }
}
