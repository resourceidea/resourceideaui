using AutoMapper;

using EastSeat.ResourceIdea.Application.Enums;
using EastSeat.ResourceIdea.Application.Features.Clients.Commands;
using EastSeat.ResourceIdea.Application.Features.Clients.Contracts;
using EastSeat.ResourceIdea.Application.Features.Clients.Validators;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Clients.Entities;
using EastSeat.ResourceIdea.Domain.Clients.Models;
using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Clients.Handlers;

public sealed class CreateClientCommandHandler(
    IClientsService clientService,
    IMapper mapper) : IRequestHandler<CreateClientCommand, ResourceIdeaResponse<ClientModel>>
{
    private readonly IClientsService _clientService = clientService;
    private readonly IMapper _mapper = mapper;

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
        var result = await _clientService.AddAsync(client, cancellationToken);

        if (result.IsFailure)
        {
            return ResourceIdeaResponse<ClientModel>.Failure(result.Error);
        }

        return _mapper.Map<ResourceIdeaResponse<ClientModel>>(result);
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