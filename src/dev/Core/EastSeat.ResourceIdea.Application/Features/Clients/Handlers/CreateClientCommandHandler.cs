using EastSeat.ResourceIdea.Application.Enums;
using EastSeat.ResourceIdea.Application.Features.Clients.Commands;
using EastSeat.ResourceIdea.Application.Features.Clients.Services;
using EastSeat.ResourceIdea.Application.Features.Clients.Validators;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Clients.Entities;
using EastSeat.ResourceIdea.Domain.Clients.Models;
using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Clients.Handlers;

public sealed class CreateClientCommandHandler(IClientsService clientService)
    : IRequestHandler<CreateClientCommand, ResourceIdeaResponse<ClientModel>>
{
    private readonly IClientsService _clientService = clientService;

    public async Task<ResourceIdeaResponse<ClientModel>> Handle(
        CreateClientCommand request,
        CancellationToken cancellationToken)
    {
        var commandValidation = await ValidateCommand(request, cancellationToken);
        if (commandValidation.IsFailure)
        {
            return commandValidation;
        }

        Client client = GetClientToCreate(request);
        return await _clientService.CreateClientAsync(client, cancellationToken);
    }

    private static Client GetClientToCreate(CreateClientCommand request)
    {
        return new()
        {
            Id = ClientId.Create(Guid.NewGuid()),
            Name = request.Name,
            Address = request.Address,
            TenantId = request.TenantId.Value
        };
    }

    private static async Task<ResourceIdeaResponse<ClientModel>> ValidateCommand(
        CreateClientCommand request,
        CancellationToken cancellationToken)
    {
        var commandValidationResponse = ResourceIdeaResponse<ClientModel>.Success(Optional<ClientModel>.None);
        
        CreateClientCommandValidator validator = new();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid is false || validationResult.Errors.Count > 0)
        {
            commandValidationResponse = ResourceIdeaResponse<ClientModel>.Failure(
                ErrorCode.CreateClientCommandValidationFailure);
        }

        return commandValidationResponse;
    }
}