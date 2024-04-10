using AutoMapper;

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Features.Tenants.Queries;
using EastSeat.ResourceIdea.Domain.Common.Responses;
using EastSeat.ResourceIdea.Domain.Tenants.Entities;
using EastSeat.ResourceIdea.Domain.Tenants.Models;

using MediatR;

using Optional;

namespace EastSeat.ResourceIdea.Application.Features.Tenants.Handlers;

/// <summary>
/// Handler for <see cref="GetTenantsListQuery"/>.
/// </summary>
public sealed class GetTenantsListQueryHandler(
    IAsyncRepository<Tenant> tenantRepository,
    IMapper mapper) : IRequestHandler<GetTenantsListQuery, ResourceIdeaResponse<PagedList<TenantModel>>>
{
    private readonly IAsyncRepository<Tenant> _tenantRepository = tenantRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ResourceIdeaResponse<PagedList<TenantModel>>> Handle(GetTenantsListQuery request, CancellationToken cancellationToken)
    {
        var specification = GetTenantsQuerySpecification(request.Filter);

        PagedList<Tenant> tenants = await _tenantRepository.GetPagedListAsync(
            request.CurrentPageNumber,
            request.PageSize,
            specification,
            cancellationToken);

        return new ResourceIdeaResponse<PagedList<TenantModel>>
        {
            Success = true,
            Content = Option.Some(_mapper.Map<PagedList<TenantModel>>(tenants))
        };
    }

    private static BaseSpecification<Tenant> GetTenantsQuerySpecification(string requestQueryFilter)
    {
        var tenantsQuerySpecification = new NoFilterSpecification<Tenant>();
        if (!string.IsNullOrEmpty(requestQueryFilter))
        {
            ; // If query filter is not empty, then create and add new specification before returning. 
        }

        return tenantsQuerySpecification;
    }
}
