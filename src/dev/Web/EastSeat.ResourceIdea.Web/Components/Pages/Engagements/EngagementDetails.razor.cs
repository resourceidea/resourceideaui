// ==================================================================================================
// File: EngagementDetails.razor.cs
// Path: src\dev\Web\EastSeat.ResourceIdea.Web\Components\Pages\Engagements\EngagementDetails.razor.cs
// Description: This file contains the code-behind for the EngagementDetails component.
// ==================================================================================================

using EastSeat.ResourceIdea.Application.Features.Engagements.Queries;
using EastSeat.ResourceIdea.Domain.Engagements.Models;
using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;
using EastSeat.ResourceIdea.Web.RequestContext;
using EastSeat.ResourceIdea.Web.Services;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace EastSeat.ResourceIdea.Web.Components.Pages.Engagements;

public partial class EngagementDetails : ComponentBase
{
    [Inject] private IResourceIdeaRequestContext ResourceIdeaRequestContext { get; set; } = null!;
    [Inject] private IMediator Mediator { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private NotificationService NotificationService { get; set; } = null!;

    [Parameter]
    public Guid Id { get; set; }
    
    private bool IsLoading { get; set; } = true;
    private bool HasError { get; set; }
    private string ErrorMessage { get; set; } = string.Empty;
    private EngagementModel? Engagement { get; set; }
    
    // Navigation tracking properties
    private string NavigationSource { get; set; } = string.Empty;
    private string? ClientId { get; set; }

    protected override async Task OnInitializedAsync()
    {
        // Parse query parameters to determine navigation source
        var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
        var queryParams = QueryHelpers.ParseQuery(uri.Query);
        
        if (queryParams.TryGetValue("from", out var fromValue))
        {
            NavigationSource = fromValue.ToString();
        }
        
        if (queryParams.TryGetValue("clientId", out var clientIdValue))
        {
            ClientId = clientIdValue.ToString();
        }

        await LoadEngagementAsync();
    }

    protected override async Task OnParametersSetAsync()
    {
        if (Id != Guid.Empty)
        {
            await LoadEngagementAsync();
        }
    }

    private async Task LoadEngagementAsync()
    {
        try
        {
            IsLoading = true;
            HasError = false;
            StateHasChanged();

            var engagementId = new EngagementId(Id);
            var query = new GetEngagementByIdQuery { EngagementId = engagementId };
            var response = await Mediator.Send(query);

            if (response.IsSuccess && response.Content.HasValue)
            {
                Engagement = response.Content.Value;
            }
            else
            {
                HasError = true;
                ErrorMessage = response.Error?.Message ?? "Failed to load engagement details.";
            }
        }
        catch (Exception ex)
        {
            HasError = true;
            ErrorMessage = $"An error occurred while loading engagement details: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
            StateHasChanged();
        }
    }

    private string GetStatusBadgeClass(EastSeat.ResourceIdea.Domain.Enums.EngagementStatus status)
    {
        return status switch
        {
            EastSeat.ResourceIdea.Domain.Enums.EngagementStatus.NotStarted => "bg-secondary",
            EastSeat.ResourceIdea.Domain.Enums.EngagementStatus.InProgress => "bg-primary",
            EastSeat.ResourceIdea.Domain.Enums.EngagementStatus.Completed => "bg-success",
            EastSeat.ResourceIdea.Domain.Enums.EngagementStatus.Canceled => "bg-danger",
            _ => "bg-secondary"
        };
    }

    private string GetBackNavigationUrl()
    {
        return NavigationSource switch
        {
            "client" when !string.IsNullOrEmpty(ClientId) => $"/clients/{ClientId}",
            "engagements" => "/engagements",
            _ => "/engagements" // Default fallback
        };
    }

    private string GetBackButtonText()
    {
        return NavigationSource switch
        {
            "client" => "Back to client details",
            "engagements" => "Back to engagements list",
            _ => "Back to engagements list" // Default fallback
        };
    }
}