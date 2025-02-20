using EastSeat.ResourceIdea.Application.Features.JobPositions.Commands;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace EastSeat.ResourceIdea.Web.Components.Pages.JobPositions;

public partial class CreateJobPosition : ComponentBase
{
    [Parameter, SupplyParameterFromQuery]
    public Guid DepartmentId { get; set; }

    [Inject] private IMediator Mediator { get; set; } = null!;    
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    
    public CreateJobPositionCommand Command { get; set; } = new();

    private async Task HandleValidSubmit()
    {
        var response = await Mediator.Send(Command);
    }
}