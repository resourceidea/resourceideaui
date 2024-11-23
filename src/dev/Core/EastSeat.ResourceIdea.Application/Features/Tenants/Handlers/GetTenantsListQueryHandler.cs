using AutoMapper;

using EastSeat.ResourceIdea.Application.Extensions;
using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Features.Tenants.Contracts;
using EastSeat.ResourceIdea.Application.Features.Tenants.Queries;
using EastSeat.ResourceIdea.Application.Features.Tenants.Specifications;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Tenants.Entities;
using EastSeat.ResourceIdea.Domain.Tenants.Models;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Tenants.Handlers;

/// <summary>
/// Handler for <see cref="GetTenantsListQuery"/>.
/// </summary>
public sealed class GetTenantsListQueryHandler(
    ITenantsService tenantsService,
    IMapper mapper) : IRequestHandler<GetTenantsListQuery, ResourceIdeaResponse<PagedListResponse<TenantModel>>>
{
    private readonly ITenantsService _tenantsService = tenantsService;
    private readonly IMapper _mapper = mapper;

    public async Task<ResourceIdeaResponse<PagedListResponse<TenantModel>>> Handle(GetTenantsListQuery request, CancellationToken cancellationToken)
    {
        var specification = GetTenantsQuerySpecification(request.Filter);

        var response = await _tenantsService.GetPagedListAsync(
            request.CurrentPageNumber,
            request.PageSize,
            specification,
            cancellationToken);

        return  _mapper.Map<ResourceIdeaResponse<PagedListResponse<TenantModel>>>(response);
    }

    private static BaseSpecification<Tenant> GetTenantsQuerySpecification(string queryFilters)
    {
        var filters = queryFilters.GetFiltersAsDictionary(delimiter: [';'], keyValueSeparator: ['=']);

        return new TenantOrganizationSpecification(filters);
    }


}
