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
using EastSeat.ResourceIdea.Web.Components.Base;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace EastSeat.ResourceIdea.Web.Components.Pages.Employees;

public partial class EmployeesTimeline : ResourceIdeaComponentBase
{
    [Inject] private IResourceIdeaRequestContext ResourceIdeaRequestContext { get; set; } = null!;
    [Inject] private IMediator Mediator { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    private List<EmployeeTimelineModel>? EmployeeTimelines { get; set; }

    // Timeline controls
    private int StartMonth { get; set; } = DateTime.Today.Month;
    private int StartYear { get; set; } = DateTime.Today.Year;
    private int EndMonth { get; set; } = DateTime.Today.Month;
    private int EndYear { get; set; } = DateTime.Today.Year;
    private string SearchTerm { get; set; } = string.Empty;

    // Modal state
    private Domain.WorkItems.Models.WorkItemModel? SelectedWorkItem { get; set; }

    protected override async Task OnInitializedAsync()
    {
        // Set default date range to current month to next 3 months
        var today = DateTime.Today;
        StartMonth = today.Month;
        StartYear = today.Year;
        
        var endDate = today.AddMonths(3);
        EndMonth = endDate.Month;
        EndYear = endDate.Year;

        await LoadTimelineData();
    }

    private async Task LoadTimelineData()
    {
        await ExecuteAsync(async () =>
        {
            var startDate = new DateOnly(StartYear, StartMonth, 1);
            var endDate = new DateOnly(EndYear, EndMonth, DateTime.DaysInMonth(EndYear, EndMonth));

            var query = new GetEmployeesTimelineQuery(startDate, endDate, SearchTerm)
            {
                TenantId = ResourceIdeaRequestContext.Tenant
            };

            var response = await Mediator.Send(query);

            EmployeeTimelines = response?.IsSuccess == true && response.Content.HasValue
                ? response.Content.Value
                : null;
        }, "Loading employees timeline");
    }

    private void NavigateToWorkItem(Guid workItemId)
    {
        NavigationManager.NavigateTo($"/workitems/{workItemId}");
    }

    private void ShowWorkItemDetails(Domain.WorkItems.Models.WorkItemModel workItem)
    {
        SelectedWorkItem = workItem;
        StateHasChanged();
    }

    private void CloseWorkItemModal()
    {
        SelectedWorkItem = null;
        StateHasChanged();
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

    private List<DateOnly> GetTimelineDates(DateOnly startDate, DateOnly endDate)
    {
        var dates = new List<DateOnly>();
        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            dates.Add(date);
        }
        return dates;
    }

    private bool IsWeekend(DateOnly date)
    {
        return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
    }

    private Domain.WorkItems.Models.WorkItemModel? GetWorkItemForDate(EmployeeTimelineModel employee, DateOnly date)
    {
        return employee.WorkItems.FirstOrDefault(wi => 
            wi.StartDate.HasValue && 
            IsDateInWorkItemRange(wi, date));
    }

    private bool IsDateInWorkItemRange(Domain.WorkItems.Models.WorkItemModel workItem, DateOnly date)
    {
        if (!workItem.StartDate.HasValue) return false;
        
        var startDate = DateOnly.FromDateTime(workItem.StartDate.Value.Date);
        var endDate = workItem.CompletedDate.HasValue 
            ? DateOnly.FromDateTime(workItem.CompletedDate.Value.Date)
            : DateOnly.FromDateTime(DateTime.Today.AddDays(365)); // Default to 1 year if no end date
        
        return date >= startDate && date <= endDate;
    }

    private string GetWorkItemDisplayText(Domain.WorkItems.Models.WorkItemModel workItem)
    {
        var displayText = "";
        
        if (!string.IsNullOrEmpty(workItem.ClientName))
        {
            displayText = workItem.ClientName;
            
            if (!string.IsNullOrEmpty(workItem.EngagementTitle))
            {
                displayText += $" - {workItem.EngagementTitle}";
            }
        }
        else if (!string.IsNullOrEmpty(workItem.EngagementTitle))
        {
            displayText = workItem.EngagementTitle;
        }
        else
        {
            displayText = workItem.Title;
        }

        // Truncate if too long for cell display
        return TruncateText(displayText, 15);
    }

    private string CalculateUtilization(EmployeeTimelineModel employee)
    {
        // Simple calculation based on work items assigned
        // In a real implementation, this would calculate based on hours or capacity
        var totalWorkItems = employee.WorkItems.Count;
        var activeWorkItems = employee.WorkItems.Count(wi => 
            wi.Status == WorkItemStatus.InProgress || wi.Status == WorkItemStatus.Completed);
        
        if (totalWorkItems == 0) return "0";
        
        var utilization = (activeWorkItems * 100.0) / totalWorkItems;
        return utilization.ToString("F1");
    }

    private string GetStatusBadgeClass(WorkItemStatus status)
    {
        return status switch
        {
            WorkItemStatus.Completed => "badge bg-success",
            WorkItemStatus.InProgress => "badge bg-warning",
            WorkItemStatus.OnHold => "badge bg-secondary",
            WorkItemStatus.Canceled => "badge bg-danger",
            _ => "badge bg-primary"
        };
    }
}