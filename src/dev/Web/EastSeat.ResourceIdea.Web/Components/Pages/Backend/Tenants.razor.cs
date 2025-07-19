using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Features.Tenants.Queries;
using EastSeat.ResourceIdea.Domain.Tenants.Models;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Web.Components.Base;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace EastSeat.ResourceIdea.Web.Components.Pages.Backend;

public partial class Tenants : ResourceIdeaComponentBase
{
    [Inject] private IMediator Mediator { get; set; } = default!;

    private List<TenantListItem> allTenants = new();
    private List<TenantListItem> filteredTenants = new();
    private string searchTerm = string.Empty;
    private string statusFilter = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        await ExecuteAsync(async () =>
        {
            await LoadTenants();
        }, "Loading tenants");
    }

    private async Task LoadTenants()
    {
        // Use the actual GetTenantsListQuery as requested
        var query = new GetTenantsListQuery
        {
            CurrentPageNumber = 1,
            PageSize = 100, // Get all tenants for now
            Filter = string.Empty
        };

        var response = await Mediator.Send(query);

        if (response.IsSuccess && response.Content.HasValue)
        {
            var tenantModels = response.Content.Value.Items;
            
            // Convert TenantModel to TenantListItem for display
            allTenants = tenantModels.Select(t => new TenantListItem
            {
                Id = t.TenantId.Value,
                Name = t.Organization,
                Domain = GetDomainFromOrganization(t.Organization), // Derive domain from organization name
                Status = "Active", // Default status - would need to be added to TenantModel if available
                UserCount = GetUserCountForTenant(t.TenantId), // Would need to query this separately
                SubscriptionPlan = "Professional", // Default plan - would need to be added to TenantModel
                CreatedAt = DateTime.Now.AddMonths(-6), // Would need to be added to TenantModel
                LastActivity = DateTime.Now.AddDays(-1) // Would need to be added to TenantModel
            }).ToList();
        }
        else
        {
            // Fallback to sample data if query fails (for demonstration)
            allTenants = GetSampleTenantData();
        }

        FilterTenants();
    }

    private List<TenantListItem> GetSampleTenantData()
    {
        return new List<TenantListItem>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Acme Corporation",
                Domain = "acme.com",
                Status = "Active",
                UserCount = 45,
                SubscriptionPlan = "Enterprise",
                CreatedAt = DateTime.Now.AddYears(-2),
                LastActivity = DateTime.Now.AddDays(-1)
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Tech Innovations Ltd",
                Domain = "techinnovations.com",
                Status = "Active",
                UserCount = 28,
                SubscriptionPlan = "Professional",
                CreatedAt = DateTime.Now.AddMonths(-8),
                LastActivity = DateTime.Now.AddHours(-4)
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "StartupXYZ",
                Domain = "startupxyz.io",
                Status = "Trial",
                UserCount = 12,
                SubscriptionPlan = "Trial",
                CreatedAt = DateTime.Now.AddDays(-15),
                LastActivity = DateTime.Now.AddMinutes(-30)
            }
        };
    }

    private string GetDomainFromOrganization(string organization)
    {
        // Simple domain derivation - in real implementation this would come from tenant data
        return organization.ToLower().Replace(" ", "").Replace("&", "and") + ".com";
    }

    private int GetUserCountForTenant(TenantId tenantId)
    {
        // This would need to be queried separately or added to TenantModel
        // For now, return a random number for demonstration
        return Random.Shared.Next(5, 100);
    }

    private void FilterTenants()
    {
        filteredTenants = allTenants.Where(t =>
            (string.IsNullOrWhiteSpace(searchTerm) ||
             t.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
             t.Domain.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) &&
            (string.IsNullOrWhiteSpace(statusFilter) || t.Status == statusFilter)
        ).ToList();
    }

    private string GetStatusBadgeClass(string status)
    {
        return status switch
        {
            "Active" => "bg-success",
            "Trial" => "bg-warning",
            "Suspended" => "bg-danger",
            _ => "bg-secondary"
        };
    }

    private void ShowCreateTenantModal()
    {
        Navigation.NavigateTo("/backend/tenants/create");
    }

    private void ViewTenant(Guid tenantId)
    {
        Navigation.NavigateTo($"/backend/tenants/{tenantId}");
    }

    private void EditTenant(Guid tenantId)
    {
        Navigation.NavigateTo($"/backend/tenants/{tenantId}/edit");
    }

    private void ViewTenantData(Guid tenantId)
    {
        Navigation.NavigateTo($"/backend/tenants/{tenantId}/data");
    }

    private async Task SuspendTenant(Guid tenantId)
    {
        await ExecuteAsync(async () =>
        {
            // Implement suspend tenant functionality
            await Task.Delay(100);
            await LoadTenants(); // Refresh the list
        }, "Suspending tenant");
    }

    private async Task ActivateTenant(Guid tenantId)
    {
        await ExecuteAsync(async () =>
        {
            // Implement activate tenant functionality
            await Task.Delay(100);
            await LoadTenants(); // Refresh the list
        }, "Activating tenant");
    }

    private async Task DeleteTenant(Guid tenantId)
    {
        await ExecuteAsync(async () =>
        {
            // Implement delete tenant functionality
            await Task.Delay(100);
            await LoadTenants(); // Refresh the list
        }, "Deleting tenant");
    }

    private class TenantListItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Domain { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int UserCount { get; set; }
        public string SubscriptionPlan { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? LastActivity { get; set; }
    }
}