using MediatR;
using EastSeat.ResourceIdea.Application.Features.Departments.Commands;
using EastSeat.ResourceIdea.Domain.Departments.Models;
using EastSeat.ResourceIdea.Domain.Types;
using EastSeat.ResourceIdea.Application.Features.Departments.Contracts;
using EastSeat.ResourceIdea.Application.Features.Tenants.Contracts;
using EastSeat.ResourceIdea.Application.Features.Departments.Validators;
using FluentValidation.Results;
using EastSeat.ResourceIdea.Domain.Departments.Entities;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Application.Mappers;

namespace EastSeat.ResourceIdea.Application.Features.Departments.Handlers;

public class UpdateDepartmentCommandHandler (
    IDepartmentsService departmentsService,
    ITenantsService tenantsService)
    : IRequestHandler<UpdateDepartmentCommand, ResourceIdeaResponse<DepartmentModel>>
{
    private readonly IDepartmentsService _departmentsService = departmentsService;
    private readonly ITenantsService _tenantsService = tenantsService;

    public async Task<ResourceIdeaResponse<DepartmentModel>> Handle(
        UpdateDepartmentCommand command,
        CancellationToken cancellationToken)
    {
        UpdateDepartmentCommandValidator validator = new();
        ValidationResult validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return ResourceIdeaResponse<DepartmentModel>.BadRequest();
        }

        Department departmentToUpdate = command.ToEntity();
        departmentToUpdate.TenantId = _tenantsService.GetTenantIdFromLoginSession(cancellationToken);
        ResourceIdeaResponse<Department> result = await _departmentsService.UpdateAsync(
            departmentToUpdate,
            cancellationToken);

        if (result.IsFailure)
        {
            return ResourceIdeaResponse<DepartmentModel>.Failure(result.Error);
        }

        if (result.Content is null)
        {
            return ResourceIdeaResponse<DepartmentModel>.Failure(ErrorCode.EmptyEntityOnUpdateDepartment);
        }

        return result.Content.ToResourceIdeaResponse();
    }
}
