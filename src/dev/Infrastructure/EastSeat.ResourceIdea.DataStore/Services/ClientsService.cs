using EastSeat.ResourceIdea.Application.Features.Clients.Contracts;
using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Domain.Clients.Entities;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.DataStore.Services;

public sealed class ClientsService : IClientsService
{
    public Task<ResourceIdeaResponse<Client>> AddAsync(Client entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ResourceIdeaResponse<Client>> DeleteAsync(Client entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ResourceIdeaResponse<Client>> GetByIdAsync(BaseSpecification<Client> specification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ResourceIdeaResponse<PagedListResponse<Client>>> GetPagedListAsync(int page, int size, Optional<BaseSpecification<Client>> specification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ResourceIdeaResponse<Client>> UpdateAsync(Client entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
