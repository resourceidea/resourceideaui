using AutoMapper;

using EastSeat.ResourceIdea.Application.Contracts.Persistence;
using EastSeat.ResourceIdea.Application.Features.Client.Commands;
using EastSeat.ResourceIdea.Application.Features.Client.DTO;
using EastSeat.ResourceIdea.Application.Features.Client.Validators;
using EastSeat.ResourceIdea.Application.Responses;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Client.Handlers;

public class CreateClientCommandHandler(IMapper mapper, IAsyncRepository<Domain.Entities.Client> clientRepository) : IRequestHandler<CreateClientCommand, BaseResponse<ClientDTO>>
{
    private readonly IMapper mapper = mapper;
    private readonly IAsyncRepository<Domain.Entities.Client> clientRepository = clientRepository;

    public async Task<BaseResponse<ClientDTO>> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<ClientDTO>();

        var validator = new CreateClientCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid || validationResult.Errors.Count > 0)
        {
            response.Success = false;
            response.Errors = [];
            foreach (var error in validationResult.Errors)
            {
                response.Errors.Add(error.ErrorMessage);
            }
        }

        if (response.Success)
        {
            var client = new Domain.Entities.Client
            {
                Id = request.Id,
                Name = request.Name,
                Address = request.Address,
                ColorCode = request.ColorCode,
                SubscriptionId = request.SubscriptionId,
                CreatedBy = request.CreatedBy
            };

            await clientRepository.AddAsync(client);
            response.Content = mapper.Map<ClientDTO>(client);
        }

        return response;
    }
}
