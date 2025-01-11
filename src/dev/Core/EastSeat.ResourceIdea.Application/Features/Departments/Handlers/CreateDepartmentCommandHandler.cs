using EastSeat.ResourceIdea.Application.Features.Clients.Commands;
using EastSeat.ResourceIdea.Application.Features.Departments.Commands;
using EastSeat.ResourceIdea.Application.Features.Departments.Contracts;
using EastSeat.ResourceIdea.Application.Features.Departments.Validators;
using EastSeat.ResourceIdea.Application.Features.Tenants.Contracts;
using EastSeat.ResourceIdea.Application.Mappers;
using EastSeat.ResourceIdea.Domain.Clients.Models;
using EastSeat.ResourceIdea.Domain.Departments.Entities;
using EastSeat.ResourceIdea.Domain.Departments.Models;
using EastSeat.ResourceIdea.Domain.Departments.ValueObjects;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Types;

using FluentValidation;
using FluentValidation.Results;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Departments.Handlers;

/// <summary>
/// Handles the command to create a department.
/// </summary>
public sealed class CreateDepartmentCommandHandler (ITenantsService tenantsService, IDepartmentsService departmentsService)
     : IRequestHandler<CreateDepartmentCommand, ResourceIdeaResponse<DepartmentModel>>
{
    private readonly ITenantsService _tenantsService = tenantsService;
    private readonly IDepartmentsService _departmentsService = departmentsService;

    public async Task<ResourceIdeaResponse<DepartmentModel>> Handle(CreateDepartmentCommand command, CancellationToken cancellationToken)
    {
        CreateDepartmentCommandValidator _validator = new();
        ValidationResult validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            return ResourceIdeaResponse<DepartmentModel>.BadRequest();
        }

        Department departmentToCreate = command.ToEntity();
        departmentToCreate.TenantId = _tenantsService.GetTenantIdFromLoginSession(cancellationToken);
        ResourceIdeaResponse<Department> result = await _departmentsService.AddAsync(departmentToCreate, cancellationToken);

        if (result.IsFailure)
        {
            return ResourceIdeaResponse<DepartmentModel>.Failure(result.Error);
        }

        if (!result.Content.HasValue)
        {
            return ResourceIdeaResponse<DepartmentModel>.Failure(ErrorCode.EmptyEntityOnCreateDepartment);
        }

        return result.Content.Value.ToResourceIdeaResponse();
    }
}

