// ====================================================================================
// File: EmployeesTimeline.razor.cs
// Path: src/dev/Web/EastSeat.ResourceIdea.Web/Components/Pages/Employees/EmployeesTimeline.razor.cs
// Description: Code-behind for the EmployeesTimeline component.
// ====================================================================================

using EastSeat.ResourceIdea.Application.Features.Employees.Queries;
using EastSeat.ResourceIdea.Domain.Employees.Models;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Types;
using EastSeat.ResourceIdea.Web.RequestContext;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace EastSeat.ResourceIdea.Web.Components.Pages.Employees;

public partial class EmployeesTimeline : ComponentBase
{
    [Inject] private IResourceIdeaRequestContext ResourceIdeaRequestContext { get; set; } = null!;
    [Inject] private IMediator Mediator { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

    private bool IsLoadingPage { get; set; } = true;
    private bool HasError { get; set; }
    private string ErrorMessage { get; set; } = string.Empty;
    private List<EmployeeTimelineModel>? EmployeeTimelines { get; set; }

    // Timeline controls
    private int StartMonth { get; set; } = DateTime.Today.Month;
    private int StartYear { get; set; } = DateTime.Today.Year;
    private int EndMonth { get; set; } = DateTime.Today.Month;
    private int EndYear { get; set; } = DateTime.Today.Year;
    private string SearchTerm { get; set; } = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        // Check authentication
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        if (!authState.User.Identity?.IsAuthenticated == true)
        {
            NavigationManager.NavigateTo("/login");
            return;
        }

        // Set default date range to current month to next 3 months
        var today = DateTime.Today;
        StartMonth = today.Month;
        StartYear = today.Year;
        
        var endDate = today.AddMonths(3);
        EndMonth = endDate.Month;
        EndYear = endDate.Year;

        await LoadTimelineData();
        IsLoadingPage = false;
        StateHasChanged();
    }

    private async Task LoadTimelineData()
    {
        IsLoadingPage = true;
        HasError = false;
        StateHasChanged();

        try
        {
            var startDate = new DateOnly(StartYear, StartMonth, 1);
            var endDate = new DateOnly(EndYear, EndMonth, DateTime.DaysInMonth(EndYear, EndMonth));

            var query = new GetEmployeesTimelineQuery(startDate, endDate, SearchTerm)
            {
                TenantId = ResourceIdeaRequestContext.Tenant
            };

            var response = await Mediator.Send(query);

            if (response?.IsSuccess == true && response.Content.HasValue)
            {
                EmployeeTimelines = response.Content.Value;
            }
            else
            {
                HasError = true;
                ErrorMessage = "Failed to load employees timeline. Please try again.";
                EmployeeTimelines = null;
            }
        }
        catch (TaskCanceledException ex)
        {
            HasError = true;
            ErrorMessage = "The operation was canceled. Please try again.";
            Console.Error.WriteLine($"Task canceled: {ex}");
        }
        catch (InvalidOperationException ex)
        {
            HasError = true;
            ErrorMessage = "An invalid operation occurred while loading the timeline.";
            Console.Error.WriteLine($"Invalid operation: {ex}");
        }
        catch (ArgumentException ex)
        {
            HasError = true;
            ErrorMessage = "An error occurred due to invalid input. Please check your date range.";
            Console.Error.WriteLine($"Argument error: {ex}");
        }
        catch (Exception ex)
        {
            HasError = true;
            ErrorMessage = "An unexpected error occurred while loading the timeline.";
            Console.Error.WriteLine($"Unexpected error: {ex}");
        }
        finally
        {
            IsLoadingPage = false;
            StateHasChanged();
        }
    }

    private void NavigateToWorkItem(Guid workItemId)
    {
        NavigationManager.NavigateTo($"/workitems/{workItemId}");
    }

    private string GetWorkItemStatusClass(WorkItemStatus status)
    {
        return status switch
        {
            WorkItemStatus.Completed => "status-completed",
            WorkItemStatus.InProgress => "status-inprogress",
            WorkItemStatus.OnHold => "status-onhold",
            WorkItemStatus.Canceled => "status-cancelled",
            _ => ""
        };
    }

    private string GetWorkItemTooltip(Domain.WorkItems.Models.WorkItemModel workItem)
    {
        var tooltip = $"Title: {workItem.Title}";
        
        if (!string.IsNullOrEmpty(workItem.Description))
        {
            tooltip += $"\nDescription: {workItem.Description}";
        }
        
        if (!string.IsNullOrEmpty(workItem.ClientName))
        {
            tooltip += $"\nClient: {workItem.ClientName}";
        }
        
        if (!string.IsNullOrEmpty(workItem.EngagementTitle))
        {
            tooltip += $"\nEngagement: {workItem.EngagementTitle}";
        }
        
        tooltip += $"\nStatus: {workItem.Status}";
        tooltip += $"\nPriority: {workItem.Priority}";
        
        if (workItem.StartDate.HasValue)
        {
            tooltip += $"\nStart Date: {workItem.StartDate.Value:MMM dd, yyyy}";
        }
        
        if (workItem.CompletedDate.HasValue)
        {
            tooltip += $"\nCompleted Date: {workItem.CompletedDate.Value:MMM dd, yyyy}";
        }

        return tooltip;
    }

    private string TruncateText(string text, int maxLength)
    {
        if (string.IsNullOrEmpty(text) || text.Length <= maxLength)
        {
            return text;
        }

        return text.Substring(0, maxLength - 3) + "...";
    }

    private string GetWorkItemStyles(Domain.WorkItems.Models.WorkItemModel workItem, DateOnly startDate, DateOnly endDate)
    {
        if (!workItem.StartDate.HasValue) return "display: none;";

        var itemStartDate = DateOnly.FromDateTime(workItem.StartDate.Value.Date);
        var itemEndDate = workItem.CompletedDate.HasValue 
            ? DateOnly.FromDateTime(workItem.CompletedDate.Value.Date) 
            : endDate;
        
        if (itemStartDate < startDate) itemStartDate = startDate;
        if (itemEndDate > endDate) itemEndDate = endDate;
        
        var totalDays = endDate.DayNumber - startDate.DayNumber + 1;
        var leftOffset = ((itemStartDate.DayNumber - startDate.DayNumber) / (double)totalDays) * 100;
        var width = ((itemEndDate.DayNumber - itemStartDate.DayNumber + 1) / (double)totalDays) * 100;

        return $"left: {leftOffset:F2}%; width: {width:F2}%;";
    }

    private string GetCurrentDateLineStyle(DateOnly currentDate, DateOnly startDate, int totalDays)
    {
        var currentDateOffset = ((currentDate.DayNumber - startDate.DayNumber) / (double)totalDays) * 100;
        return $"left: {currentDateOffset:F2}%";
    }
}