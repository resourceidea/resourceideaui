using AutoMapper;
using EastSeat.ResourceIdea.Application.Constants;
using EastSeat.ResourceIdea.Application.Features.Clients.Commands;
using EastSeat.ResourceIdea.Application.Features.Clients.Validators;
using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Clients.Entities;
using EastSeat.ResourceIdea.Domain.Clients.Models;
using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Clients.Handlers;

public sealed class CreateClientCommandHandler(
    IAsyncRepository<Client> clientRepository,
    IMapper mapper)
    : IRequestHandler<CreateClientCommand, ResourceIdeaResponse<ClientModel>>
{
    private readonly IAsyncRepository<Client> _clientRepository = clientRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ResourceIdeaResponse<ClientModel>> Handle(
        CreateClientCommand request,
        CancellationToken cancellationToken)
    {
        CreateClientCommandValidator validator = new();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid || validationResult.Errors.Count > 0)
        {
            return new ResourceIdeaResponse<ClientModel>
            {
                Success = false,
                Message = "Create client command validation failed",
                ErrorCode = ErrorCodes.CreateClientCommandValidationFailure.ToString(),
                Content = Optional<ClientModel>.None
            };
        }

        Client client = new()
        {
            Id = ClientId.Create(Guid.NewGuid()),
            Name = request.Name,
            Address = request.Address,
            TenantId = request.TenantId.Value
        };
        var addedClient = await _clientRepository.AddAsync(client, cancellationToken);

        return new ResourceIdeaResponse<ClientModel>
        {
            Success = true,
            Message = "Client created successfully",
            Content = Optional<ClientModel>.Some(_mapper.Map<ClientModel>(addedClient))
        };
    }
}