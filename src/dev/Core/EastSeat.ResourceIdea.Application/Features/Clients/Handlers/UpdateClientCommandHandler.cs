using EastSeat.ResourceIdea.Application.Features.Clients.Commands;
using EastSeat.ResourceIdea.Application.Features.Clients.Contracts;
using EastSeat.ResourceIdea.Application.Features.Clients.Validators;
using EastSeat.ResourceIdea.Application.Features.Tenants.Contracts;
using EastSeat.ResourceIdea.Application.Mappers;
using EastSeat.ResourceIdea.Domain.Clients.Entities;
using EastSeat.ResourceIdea.Domain.Clients.Models;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Types;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Clients.Handlers;

/// <summary>
/// Handles command to update a client.
/// </summary>
public sealed class UpdateClientCommandHandler (IClientsService clientsService, ITenantsService tenantsService)
    : IRequestHandler<UpdateClientCommand, ResourceIdeaResponse<ClientModel>>
{
    private readonly IClientsService _clientsService = clientsService;
    private readonly ITenantsService _tenantsService = tenantsService;

    public async Task<ResourceIdeaResponse<ClientModel>> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
    {
        var commandValidation = await ValidateCommand(request, cancellationToken);
        if (commandValidation.IsFailure)
        {
            return commandValidation;
        }

        Client client = request.ToEntity();
        client.TenantId = _tenantsService.GetTenantIdFromLoginSession(cancellationToken);
        var response = await _clientsService.UpdateAsync(client, cancellationToken);
        if (response.IsFailure)
        {
            return ResourceIdeaResponse<ClientModel>.Failure(response.Error);
        }

        if (response.Content.HasValue is false)
        {
            return ResourceIdeaResponse<ClientModel>.Failure(ErrorCode.EmptyEntityOnUpdateClient);
        }

        return response.Content.Value.ToResourceIdeaResponse();
    }

    private static async Task<ResourceIdeaResponse<ClientModel>> ValidateCommand(UpdateClientCommand request, CancellationToken cancellationToken)
    {
        var commandValidationResponse = ResourceIdeaResponse<ClientModel>.Success(Optional<ClientModel>.None);

        UpdateClientCommandValidator validator = new();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid is false || validationResult.Errors.Count > 0)
        {
            commandValidationResponse = ResourceIdeaResponse<ClientModel>.Failure(ErrorCode.UpdateClientCommandValidationFailure);
        }

        return commandValidationResponse;
    }
}