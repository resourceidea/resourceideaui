using Microsoft.AspNetCore.Components;
using MediatR;
using EastSeat.ResourceIdea.Application.Features.Departments.Queries;
using EastSeat.ResourceIdea.Application.Features.Departments.Commands;
using EastSeat.ResourceIdea.Domain.Departments.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;
using EastSeat.ResourceIdea.Domain.Departments.Models;
using System.Threading.Tasks;

namespace EastSeat.ResourceIdea.Web.Components.Pages.Departments;

public partial class DepartmentDetail : ComponentBase
{
    [Parameter] public Guid Id { get; set; }

    public UpdateDepartmentCommand Model { get; set; } = new();

    private bool IsLoadingModelData;
    private string? errorMessage;
    private bool isErrorMessage;

    [Inject] private IMediator Mediator { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        IsLoadingModelData = true;

        GetDepartmentByIdQuery query = new() { DepartmentId = DepartmentId.Create(Id) };
        ResourceIdeaResponse<DepartmentModel> response = await Mediator.Send(query);
        if (response.IsSuccess && response.Content.HasValue)
        {
            Model = new UpdateDepartmentCommand
            {
                Name = response.Content.Value.Name,
                DepartmentId = response.Content.Value.DepartmentId
            };
        }
        else
        {
            // TODO: Log failure to query for department.                
            DisplayMessage(
                message: "Failed to query for department.",
                isError: true);
        }

        IsLoadingModelData = false;
    }

    protected async Task HandleValidSubmit()
    {
        ResourceIdeaResponse<DepartmentModel> response = await Mediator.Send(Model);
        if (response.IsSuccess && response.Content.HasValue)
        {
            DisplayMessage(
                message: "Changes saved successfully",
                isError: false);
        }
        else
        {
            DisplayMessage(
                message: "Failed to save changes",
                isError: true);
        }
    }

    private void DisplayMessage(string message, bool isError)
    {
        errorMessage = message;
        isErrorMessage = isError;
    }
}
