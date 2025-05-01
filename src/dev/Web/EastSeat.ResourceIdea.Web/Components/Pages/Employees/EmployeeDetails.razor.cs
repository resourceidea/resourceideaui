using EastSeat.ResourceIdea.Application.Features.Departments.Queries;
using EastSeat.ResourceIdea.Application.Features.Employees.Commands;
using EastSeat.ResourceIdea.Application.Features.Employees.Queries;
using EastSeat.ResourceIdea.Application.Features.JobPositions.Queries;
using EastSeat.ResourceIdea.Domain.Departments.Models;
using EastSeat.ResourceIdea.Domain.Departments.ValueObjects;
using EastSeat.ResourceIdea.Domain.Employees.ValueObjects;
using EastSeat.ResourceIdea.Domain.JobPositions.Models;
using EastSeat.ResourceIdea.Web.RequestContext;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace EastSeat.ResourceIdea.Web.Components.Pages.Employees;

public partial class EmployeeDetails : ComponentBase
{
    [Parameter]
    public Guid Id { get; set; }

    [Inject] private IMediator Mediator { get; set; } = null!;
    [Inject] private IResourceIdeaRequestContext ResourceIdeaRequestContext { get; set; } = null!;

    private string? message;
    private bool isErrorMessage;
    private bool IsLoadingModelData = true;

    public UpdateEmployeeCommand Command { get; set; } = new();
    public List<DepartmentModel> Departments { get; private set; } = [];
    public List<JobPositionModel> JobPositions { get; private set; } = [];

    protected override async Task OnInitializedAsync()
    {
        await LoadDepartments();
        await LoadEmployeeData();
    }

    private async Task LoadEmployeeData()
    {
        IsLoadingModelData = true;

        try
        {
            var query = new GetEmployeeByIdQuery
            {
                EmployeeId = EmployeeId.Create(Id),
                TenantId = ResourceIdeaRequestContext.Tenant
            };

            var response = await Mediator.Send(query);
            if (response.IsSuccess && response.Content.Value is not null)
            {
                var employee = response.Content.Value;
                Command.EmployeeId = employee.EmployeeId;
                Command.FirstName = employee.FirstName;
                Command.LastName = employee.LastName;
                Command.Email = employee.Email;
                Command.DepartmentId = employee.DepartmentId;
                Command.JobPositionId = employee.JobPositionId;
                Command.ApplicationUserId = employee.ApplicationUserId;
                Command.EmployeeNumber = employee.EmployeeNumber;
                if (Command.DepartmentId != DepartmentId.Empty)
                {
                    await LoadJobPositions();
                }
            }
            else
            {
                message = "Employee not found";
                isErrorMessage = true;
            }
        }
        catch (Exception ex)
        {
            message = $"Error loading employee data: {ex.Message}";
            isErrorMessage = true;
        }
        finally
        {
            IsLoadingModelData = false;
            StateHasChanged();
        }
    }

    private async Task LoadDepartments()
    {
        try
        {
            var query = new GetAllDepartmentsQuery
            {
                TenantId = ResourceIdeaRequestContext.Tenant
            };

            var response = await Mediator.Send(query);
            if (response.IsSuccess && response.Content.Value is not null)
            {
                Departments = [.. response.Content.Value.Items];
            }
        }
        catch (Exception ex)
        {
            message = $"Error loading departments: {ex.Message}";
            isErrorMessage = true;
        }
    }

    private async Task LoadJobPositions()
    {
        if (Command.DepartmentId == DepartmentId.Empty)
        {
            JobPositions.Clear();
            return;
        }

        try
        {
            var departmentId = Command.DepartmentId;
            var query = new GetJobPositionsByDepartmentIdQuery
            {
                DepartmentId = departmentId,
                TenantId = ResourceIdeaRequestContext.Tenant
            };

            var response = await Mediator.Send(query);
            if (response.IsSuccess && response.Content.Value is not null)
            {
                JobPositions = [.. response.Content.Value.Items.Select(j => new JobPositionModel
                {
                    Id = j.JobPositionId,
                    Title = j.Title ?? string.Empty,
                    DepartmentId = departmentId,
                })];
            }
            else
            {
                JobPositions.Clear();
            }
        }
        catch (Exception ex)
        {
            message = $"Error loading job positions: {ex.Message}";
            isErrorMessage = true;
            JobPositions.Clear();
        }

        StateHasChanged();
    }

    private async Task HandleValidSubmit()
    {
        try
        {
            Command.TenantId = ResourceIdeaRequestContext.Tenant;
            var response = await Mediator.Send(Command);
            if (response.IsSuccess)
            {
                message = "Employee details updated successfully";
                isErrorMessage = false;
            }
            else
            {
                message = $"Failed to update employee details";
                isErrorMessage = true;
            }
        }
        catch (Exception ex)
        {
            message = $"Error updating employee: {ex.Message}";
            isErrorMessage = true;
        }

        StateHasChanged();
    }

    private void ClearMessage()
    {
        message = null;
        StateHasChanged();
    }
}
