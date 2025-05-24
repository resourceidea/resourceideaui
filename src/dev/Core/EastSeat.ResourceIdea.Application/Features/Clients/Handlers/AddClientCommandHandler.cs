// ===============================================================================
// File: AddClientCommandHandler.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\Clients\Handlers\AddClientCommandHandler.cs
// Description: Handles the command to add a new client.
// ===============================================================================

using EastSeat.ResourceIdea.Application.Features.Clients.Commands;
using EastSeat.ResourceIdea.Application.Features.Clients.Contracts;
using EastSeat.ResourceIdea.Application.Features.Common.Handlers;
using EastSeat.ResourceIdea.Domain.Clients.Entities;
using EastSeat.ResourceIdea.Domain.Clients.Models;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Types;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Clients.Handlers;

public class AddClientCommandHandler(IClientsService clientsService) :
    BaseHandler,
    IRequestHandler<AddClientCommand, ResourceIdeaResponse<ClientModel>>
{
    private readonly IClientsService _clientsService = clientsService;

    public async Task<ResourceIdeaResponse<ClientModel>> Handle(
        AddClientCommand command,
        CancellationToken cancellationToken)
    {
        ValidationResponse commandValidation = command.Validate();
        if (!commandValidation.IsValid && commandValidation.ValidationFailureMessages.Any())
        {
            // TODO: Log validation failure.
            return ResourceIdeaResponse<ClientModel>.Failure(ErrorCode.CommandValidationFailure);
        }

        Client client = command.ToEntity();
        var addClientResponse = await _clientsService.AddAsync(client, cancellationToken);
        if (addClientResponse.IsFailure)
        {
            // TODO: Log failure to add a client.
            return ResourceIdeaResponse<ClientModel>.Failure(addClientResponse.Error);
        }

        return addClientResponse.Content.Value.ToResourceIdeaResponse<Client, ClientModel>();
    }
}
