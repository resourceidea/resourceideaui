using EastSeat.ResourceIdea.Application.Features.Common.Handlers;
using EastSeat.ResourceIdea.Application.Features.Departments.Contracts;
using EastSeat.ResourceIdea.Application.Features.Departments.Queries;
using EastSeat.ResourceIdea.Application.Features.Departments.Specifications;
using EastSeat.ResourceIdea.Application.Mappers;
using EastSeat.ResourceIdea.Domain.Departments.Models;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Departments.Handlers;

/// <summary>
/// Handles querying for a department given the ID of the department.
/// </summary>
/// <param name="departmentsService">Service that provides interface to departments data.</param>
public sealed class GetDepartmentByIdQueryHandler(IDepartmentsService departmentsService)
    : BaseHandler, IRequestHandler<GetDepartmentByIdQuery, ResourceIdeaResponse<DepartmentModel>>
{
    private readonly IDepartmentsService _departmentsService = departmentsService;

    /// <inheritdoc />
    public async Task<ResourceIdeaResponse<DepartmentModel>> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
    {
        TenantId tenantId = GetTenantIdFromLoginSession();
        var departmentIdSpecification = new DepartmentIdSpecification(request.DepartmentId, tenantId);
        var response = await _departmentsService.GetByIdAsync(departmentIdSpecification, cancellationToken);
        if (response.IsFailure)
        {
            return ResourceIdeaResponse<DepartmentModel>.Failure(response.Error);
        }

        if (response.Content.HasValue is false)
        {
            return ResourceIdeaResponse<DepartmentModel>.NotFound();
        }

        return response.Content.Value.ToResourceIdeaResponse();
    }
}
