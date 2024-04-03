using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Features.TenantManagement.Queries;
using EastSeat.ResourceIdea.Domain.Common.Responses;
using EastSeat.ResourceIdea.Domain.Tenant.Entities;
using EastSeat.ResourceIdea.Domain.Tenant.Models;

using MediatR;

using Optional;
using Optional.Linq;

namespace EastSeat.ResourceIdea.Application.Features.TenantManagement.Handlers;

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
