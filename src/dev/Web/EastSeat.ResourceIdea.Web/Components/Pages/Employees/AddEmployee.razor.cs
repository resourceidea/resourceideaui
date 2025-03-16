using System;
using System.ComponentModel;
using EastSeat.ResourceIdea.Application.Features.Employees.Commands;
using EastSeat.ResourceIdea.Web.RequestContext;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace EastSeat.ResourceIdea.Web.Components.Pages.Employees;

public partial class AddEmployee : ComponentBase
{
    private string? errorMessage;
    private bool isErrorMessage;
    [Inject] private IMediator Mediator { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IResourceIdeaRequestContext ResourceIdeaRequestContext { get; set; } = null!;

    [Parameter, SupplyParameterFromQuery] public Guid JobPositionId { get; set; }
    [Parameter, SupplyParameterFromQuery] public string? ReturnView { get; set; }
    [Parameter, SupplyParameterFromQuery] public string? ReturnId { get; set; }

    public AddEmployeeCommand Command { get; set; } = new();

    private Task HandleValidSubmit()
    {
        throw new NotImplementedException("HandleValidSubmit not implemented yet.");
    }
}
