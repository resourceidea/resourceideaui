// =============================================================================================
// File: CreateClientCommandHandler.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\Clients\Handlers\CreateClientCommandHandler.cs
// Description: Command handler for creating a client.
// =============================================================================================

using EastSeat.ResourceIdea.Application.Features.Clients.Commands;
using EastSeat.ResourceIdea.Application.Features.Clients.Contracts;
using EastSeat.ResourceIdea.Application.Features.Common.Handlers;
using EastSeat.ResourceIdea.Domain.Clients.Entities;
using EastSeat.ResourceIdea.Domain.Clients.Models;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Types;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Clients.Handlers;

/// <summary>
/// Handles the command to create a client.
/// </summary>
/// <param name="clientService"></param>
/// <param name="tenantsService"></param>
public sealed class CreateClientCommandHandler(IClientsService clientService)
    : BaseHandler,
      IRequestHandler<CreateClientCommand, ResourceIdeaResponse<ClientModel>>
{
    private readonly IClientsService _clientService = clientService;

    public async Task<ResourceIdeaResponse<ClientModel>> Handle(
        CreateClientCommand command,
        CancellationToken cancellationToken)
    {
        ValidationResponse commandValidation = command.Validate();
        if (commandValidation.IsValid is false && commandValidation.ValidationFailureMessages.Any())
        {
            // TODO: Log validation failure.
            return ResourceIdeaResponse<ClientModel>.Failure(ErrorCode.CommandValidationFailure);
        }

        Client client = command.ToEntity();
        var addClientResponse = await _clientService.AddAsync(client, cancellationToken);
        if (addClientResponse.IsFailure)
        {
            // TODO: Log failure to add a client.
            return ResourceIdeaResponse<ClientModel>.Failure(addClientResponse.Error);
        }

        return addClientResponse.Content.Value.ToResourceIdeaResponse<Client, ClientModel>();
    }
}