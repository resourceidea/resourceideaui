using EastSeat.ResourceIdea.Application.Features.Departments.Queries;
using EastSeat.ResourceIdea.Application.Features.Employees.Commands;
using EastSeat.ResourceIdea.Application.Features.Employees.Queries;
using EastSeat.ResourceIdea.Application.Features.JobPositions.Queries;
using EastSeat.ResourceIdea.Domain.Departments.Models;
using EastSeat.ResourceIdea.Domain.Departments.ValueObjects;
using EastSeat.ResourceIdea.Domain.Employees.ValueObjects;
using EastSeat.ResourceIdea.Domain.JobPositions.Models;
using EastSeat.ResourceIdea.Domain.Employees.Models;
using EastSeat.ResourceIdea.Web.RequestContext;
using EastSeat.ResourceIdea.Web.Components.Base;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace EastSeat.ResourceIdea.Web.Components.Pages.Employees;

public partial class EmployeeDetails : ResourceIdeaComponentBase
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
    public List<TenantEmployeeModel> PotentialManagers { get; private set; } = [];

    protected override async Task OnInitializedAsync()
    {
        await LoadDepartments();
        await LoadPotentialManagers();
        await LoadEmployeeData();
    }

    private async Task LoadEmployeeData()
    {
        IsLoadingModelData = true;

        await ExecuteAsync(async () =>
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
                Command.ManagerId = employee.ManagerId;
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
        }, "Loading employee data", manageLoadingState: false);

        IsLoadingModelData = false;
        StateHasChanged();
    }

    private async Task LoadDepartments()
    {
        await ExecuteAsync(async () =>
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
            else
            {
                throw new InvalidOperationException("Failed to load departments");
            }
        }, "Loading departments", manageLoadingState: false);
    }

    private async Task LoadJobPositions()
    {
        if (Command.DepartmentId == DepartmentId.Empty)
        {
            JobPositions.Clear();
            return;
        }

        await ExecuteAsync(async () =>
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
        }, "Loading job positions", manageLoadingState: false);

        StateHasChanged();
    }

    private async Task LoadPotentialManagers()
    {
        await ExecuteAsync(async () =>
        {
            var query = new GetPotentialManagersQuery
            {
                ExcludeEmployeeId = EmployeeId.Create(Id),
                TenantId = ResourceIdeaRequestContext.Tenant,
                PageSize = 100 // Get all potential managers for dropdown
            };

            var response = await Mediator.Send(query);
            if (response.IsSuccess && response.Content.Value is not null)
            {
                PotentialManagers = [.. response.Content.Value.Items];
            }
            else
            {
                PotentialManagers.Clear();
            }
        }, "Loading potential managers", manageLoadingState: false);
    }

    private async Task HandleValidSubmit()
    {
        await ExecuteAsync(async () =>
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
                message = "Failed to update employee details";
                isErrorMessage = true;
            }
        }, "Updating employee details", manageLoadingState: false);

        StateHasChanged();
    }

    private void ClearMessage()
    {
        message = null;
        StateHasChanged();
    }

    private async Task ResetPassword()
    {
        if (string.IsNullOrEmpty(Command.Email))
        {
            message = "Employee email is required to reset password";
            isErrorMessage = true;
            StateHasChanged();
            return;
        }

        await ExecuteAsync(async () =>
        {
            var resetCommand = new ResetEmployeePasswordCommand
            {
                EmployeeId = Command.EmployeeId,
                Email = Command.Email,
                TenantId = ResourceIdeaRequestContext.Tenant
            };

            var response = await Mediator.Send(resetCommand);
            if (response.IsSuccess && response.Content.Value is not null)
            {
                var temporaryPassword = response.Content.Value;
                message = $"Password reset successfully. Temporary password: {temporaryPassword}. The user should change this password on next login.";
                isErrorMessage = false;
            }
            else
            {
                message = "Failed to reset password. Please try again.";
                isErrorMessage = true;
            }
        }, "Resetting password", manageLoadingState: false);

        StateHasChanged();
    }
}
