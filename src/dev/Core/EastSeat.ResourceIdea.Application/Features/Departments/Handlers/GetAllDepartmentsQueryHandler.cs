using EastSeat.ResourceIdea.Application.Extensions;
using EastSeat.ResourceIdea.Application.Features.Common.Handlers;
using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Features.Departments.Contracts;
using EastSeat.ResourceIdea.Application.Features.Departments.Queries;
using EastSeat.ResourceIdea.Application.Features.Departments.Specifications;
using EastSeat.ResourceIdea.Domain.Departments.Entities;
using EastSeat.ResourceIdea.Domain.Departments.Models;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Departments.Handlers;

/// <summary>
/// Handles the query to get all departments with pagination and optional filtering.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="GetAllDepartmentsQueryHandler"/> class.
/// </remarks>
/// <param name="departmentsService">The service to handle department operations.</param>
public sealed class GetAllDepartmentsQueryHandler(IDepartmentsService departmentsService)
    : BaseHandler, IRequestHandler<GetAllDepartmentsQuery, ResourceIdeaResponse<PagedListResponse<DepartmentModel>>>
{
    private readonly IDepartmentsService _departmentsService = departmentsService;

    /// <summary>
    /// Handles the query to get all departments.
    /// </summary>
    /// <param name="query">The query containing pagination and filter information.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the response with the paged list of department models.</returns>
    public async Task<ResourceIdeaResponse<PagedListResponse<DepartmentModel>>> Handle(GetAllDepartmentsQuery query, CancellationToken cancellationToken)
    {
        BaseSpecification<Department> specification = GetDepartmentsQuerySpecification(query.Filter);
        ResourceIdeaResponse<PagedListResponse<Department>> serviceResponse = await _departmentsService.GetPagedListAsync(query.PageNumber, query.PageSize, specification, cancellationToken);
        ResourceIdeaResponse<PagedListResponse<DepartmentModel>> handlerResponse = GetHandlerResponse<Department, DepartmentModel>(serviceResponse);

        return handlerResponse;
    }

    /// <summary>
    /// Gets the specification for querying departments based on the provided filters.
    /// </summary>
    /// <param name="filters">The filter string to filter the departments.</param>
    /// <returns>The specification for querying departments.</returns>
    private static BaseSpecification<Department> GetDepartmentsQuerySpecification(string filters)
    {
        TenantId tenantId = GetTenantIdFromLoginSession();
        BaseSpecification<Department> specification = new TenantIdSpecification<Department>(tenantId);

        var queryFilters = filters.GetFiltersAsDictionary(delimiter: [';'], keyValueSeparator: ['=']);
        if (queryFilters == null || queryFilters.Count == 0)
        {
            return specification;
        }

        if (queryFilters.ContainsKey("name"))
        {
            specification.And(new DepartmentNameSpecification(queryFilters));
        }

        return specification;
    }
}
