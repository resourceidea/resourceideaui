using AutoMapper;

using EastSeat.ResourceIdea.Application.Contracts.Persistence;
using EastSeat.ResourceIdea.Application.Features.Client.DTO;
using EastSeat.ResourceIdea.Application.Features.Client.Queries;
using EastSeat.ResourceIdea.Application.Responses;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Client.Handlers;

public class GetClientByIdQueryHandler(IMapper mapper, IAsyncRepository<Domain.Entities.Client> clientRepository) : IRequestHandler<GetClientByIdQuery, BaseResponse<ClientDTO>>
{
    public async Task<BaseResponse<ClientDTO>> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
    {
        var client = await clientRepository.GetByIdAsync(request.Id);
        if (client is null)
        {
            return new BaseResponse<ClientDTO>
            {
                Success = false,
                Message = $"Client with Id {request.Id} not found.",
                ErrorCode = Constants.ErrorCodes.NotFound,
                Errors = [
                    Constants.ErrorCodes.NotFound
                    ]
            };
        }
        return new BaseResponse<ClientDTO>
        {
            Success = true,
            Content = mapper.Map<ClientDTO>(client)
        };
    }
}
