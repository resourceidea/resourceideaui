using EastSeat.ResourceIdea.Application.Enums;
using EastSeat.ResourceIdea.Application.Features.Clients.Commands;
using EastSeat.ResourceIdea.Application.Features.Clients.Contracts;
using EastSeat.ResourceIdea.Application.Features.Clients.Validators;
using EastSeat.ResourceIdea.Application.Features.Tenants.Contracts;
using EastSeat.ResourceIdea.Application.Mappers;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Clients.Entities;
using EastSeat.ResourceIdea.Domain.Clients.Models;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Clients.Handlers;

/// <summary>
/// Handles the command to create a client.
/// </summary>
/// <param name="clientService"></param>
/// <param name="tenantsService"></param>
public sealed class CreateClientCommandHandler(IClientsService clientService, ITenantsService tenantsService)
    : IRequestHandler<CreateClientCommand, ResourceIdeaResponse<ClientModel>>
{
    private readonly IClientsService _clientService = clientService;
    private readonly ITenantsService _tenantsService = tenantsService;

    public async Task<ResourceIdeaResponse<ClientModel>> Handle(
        CreateClientCommand request,
        CancellationToken cancellationToken)
    {
        var commandValidation = await ValidateCommand(request, cancellationToken);
        if (commandValidation.IsFailure)
        {
            return commandValidation;
        }

        Client client = request.ToEntity();
        client.TenantId = _tenantsService.GetTenantIdFromLoginSession(cancellationToken);
        var result = await _clientService.AddAsync(client, cancellationToken);

        if (result.IsFailure)
        {
            return ResourceIdeaResponse<ClientModel>.Failure(result.Error);
        }

        if (result.Content.HasValue is false)
        {
            return ResourceIdeaResponse<ClientModel>.Failure(ErrorCode.CreateClientCommandValidationFailure);
        }

        return result.Content.Value.ToResourceIdeaResponse();
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