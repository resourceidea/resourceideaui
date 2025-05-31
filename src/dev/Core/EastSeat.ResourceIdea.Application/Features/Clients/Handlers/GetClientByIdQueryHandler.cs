using EastSeat.ResourceIdea.Application.Features.Clients.Contracts;
using EastSeat.ResourceIdea.Application.Features.Clients.Queries;
using EastSeat.ResourceIdea.Application.Features.Clients.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.Handlers;
using EastSeat.ResourceIdea.Domain.Clients.Entities;
using EastSeat.ResourceIdea.Domain.Clients.Models;
using EastSeat.ResourceIdea.Domain.Employees.Models;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Types;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Clients.Handlers;

/// <summary>
/// Handles the query to get a client by id.
/// </summary>
/// <param name="clientsService"></param>
public sealed class GetClientByIdQueryHandler(IClientsService clientsService)
    : BaseHandler, IRequestHandler<GetClientByIdQuery, ResourceIdeaResponse<ClientModel>>
{
    private readonly IClientsService _clientsService = clientsService;

    public async Task<ResourceIdeaResponse<ClientModel>> Handle(
        GetClientByIdQuery query,
        CancellationToken cancellationToken)
    {
        ValidationResponse queryValidation = query.Validate();
        if (!queryValidation.IsValid && queryValidation.ValidationFailureMessages.Any())
        {
            return ResourceIdeaResponse<ClientModel>.Failure(ErrorCode.ClientQueryValidationFailure);
        }

        var getClientByIdSpecification = new ClientGetByIdSpecification(query.ClientId, query.TenantId);
        ResourceIdeaResponse<Client> result = await _clientsService.GetByIdAsync(getClientByIdSpecification, cancellationToken);

        return result switch
        {
            null => ResourceIdeaResponse<ClientModel>.Failure(ErrorCode.NotFound),
            { IsFailure: true } => ResourceIdeaResponse<ClientModel>.Failure(result.Error),
            { Content.HasValue: false } => ResourceIdeaResponse<ClientModel>.NotFound(),
            _ => result.Content.Value.ToResourceIdeaResponse<Client, ClientModel>()
        };
    }
}