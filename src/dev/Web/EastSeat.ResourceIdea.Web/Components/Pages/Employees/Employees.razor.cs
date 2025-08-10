// ====================================================================================
// File: Employees.razor.cs
// Path: src/dev/Web/EastSeat.ResourceIdea.Web/Components/Pages/Employees/Employees.razor.cs
// Description: Code-behind for Employees List Page
// ====================================================================================

using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Features.Employees.Queries;
using EastSeat.ResourceIdea.Domain.Employees.Models;
using EastSeat.ResourceIdea.Web.Components.Base;
using EastSeat.ResourceIdea.Web.RequestContext;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace EastSeat.ResourceIdea.Web.Components.Pages.Employees;

/// <summary>
/// Code-behind for the Employees list page component.
/// </summary>
public partial class Employees : ResourceIdeaComponentBase
{
    [Inject] private IMediator Mediator { get; set; } = default!;
    [Inject] private IResourceIdeaRequestContext ResourceIdeaRequestContext { get; set; } = default!;

    private bool IsLoadingModelData { get; set; } = true;
    private PagedListResponse<TenantEmployeeModel>? TenantEmployees { get; set; }

    /// <summary>
    /// Initializes the component and loads employees data.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected override async Task OnInitializedAsync() => await LoadEmployeesAsync();

    /// <summary>
    /// Handles page change events from the employees list component.
    /// </summary>
    /// <param name="page">The page number to navigate to.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected async Task HandlePageChangeAsync(int page) => await LoadEmployeesAsync(page);

    /// <summary>
    /// Loads employees data for the specified page.
    /// </summary>
    /// <param name="page">The page number to load (default is 1).</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task LoadEmployeesAsync(int page = 1)
    {
        var success = await ExecuteAsync(async () =>
        {
            var query = new TenantEmployeesQuery
            {
                TenantId = ResourceIdeaRequestContext.Tenant,
                PageNumber = page,
                PageSize = 10
            };

            var response = await Mediator.Send(query);
            if (response.IsSuccess && response.Content.HasValue)
            {
                TenantEmployees = response.Content.Value;
            }
            else
            {
                // Error handling is now managed by ExecuteAsync
                TenantEmployees = null;
            }

            // Only set loading state after successful data load
            IsLoadingModelData = false;
        }, $"Loading employees page {page}");

        // Only call StateHasChanged if the operation was successful
        if (success)
        {
            SafeStateHasChanged();
        }
    }
}
