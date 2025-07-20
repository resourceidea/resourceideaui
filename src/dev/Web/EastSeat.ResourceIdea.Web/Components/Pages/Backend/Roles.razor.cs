using EastSeat.ResourceIdea.DataStore.Identity.Entities;
using EastSeat.ResourceIdea.Web.Components.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EastSeat.ResourceIdea.Web.Components.Pages.Backend;

public partial class Roles : ResourceIdeaComponentBase
{
    [Inject] private RoleManager<ApplicationRole> RoleManager { get; set; } = default!;

    private List<RoleListItem> allRoles = new();
    private List<RoleListItem> filteredRoles = new();
    private RoleType selectedRoleType = RoleType.All;

    // Claims modal state
    private bool showClaimsModal = false;
    private RoleListItem? selectedRole = null;
    private List<ClaimListItem> roleClaims = new();
    private bool showAddClaimForm = false;
    private string newClaimType = string.Empty;
    private string newClaimValue = string.Empty;

    // Create role modal state
    private bool showCreateRoleModal = false;
    private string newRoleName = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        await ExecuteAsync(async () =>
        {
            await LoadRoles();
        }, "Loading roles");
    }

    private async Task LoadRoles()
    {
        var roles = await RoleManager.Roles.ToListAsync();
        var roleListItems = new List<RoleListItem>();

        foreach (var role in roles)
        {
            var claims = await RoleManager.GetClaimsAsync(role);
            // Note: Getting user count is complex with Identity, so we'll skip this for now
            // In a real implementation, you would need UserManager to check users in role
            var usersInRole = 0;

            roleListItems.Add(new RoleListItem
            {
                Id = role.Id,
                Name = role.Name ?? "Unknown",
                IsBackendRole = role.IsBackendRole,
                TenantId = role.TenantId,
                TenantName = role.TenantId != Domain.Tenants.ValueObjects.TenantId.Empty ? GetTenantName(role.TenantId) : null,
                ClaimsCount = claims.Count,
                HasUsers = usersInRole > 0
            });
        }

        allRoles = roleListItems.OrderBy(r => r.Name).ToList();
        FilterByRoleType(selectedRoleType);
    }

    private string? GetTenantName(Domain.Tenants.ValueObjects.TenantId tenantId)
    {
        // For now, return a placeholder. In a real implementation, 
        // you would query the tenant by ID to get the name
        return $"Tenant {tenantId.Value}";
    }

    private void FilterByRoleType(RoleType roleType)
    {
        selectedRoleType = roleType;

        filteredRoles = roleType switch
        {
            RoleType.System => allRoles.Where(r => r.IsSystemRole).ToList(),
            RoleType.Tenant => allRoles.Where(r => !r.IsSystemRole).ToList(),
            _ => allRoles.ToList()
        };
    }

    private async Task ViewRoleClaims(string roleId)
    {
        await ExecuteAsync(async () =>
        {
            var role = await RoleManager.FindByIdAsync(roleId);
            if (role != null)
            {
                selectedRole = allRoles.First(r => r.Id == roleId);
                var claims = await RoleManager.GetClaimsAsync(role);
                
                roleClaims = claims.Select((claim, index) => new ClaimListItem
                {
                    Id = index, // Using index as ID since IdentityRoleClaim doesn't expose ID easily
                    ClaimType = claim.Type,
                    ClaimValue = claim.Value ?? string.Empty
                }).ToList();

                showClaimsModal = true;
            }
        }, "Loading role claims");
    }

    private void CloseClaimsModal()
    {
        showClaimsModal = false;
        selectedRole = null;
        roleClaims.Clear();
        showAddClaimForm = false;
        newClaimType = string.Empty;
        newClaimValue = string.Empty;
    }

    private void ShowAddClaimForm()
    {
        showAddClaimForm = true;
    }

    private void CancelAddClaim()
    {
        showAddClaimForm = false;
        newClaimType = string.Empty;
        newClaimValue = string.Empty;
    }

    private async Task AddClaim()
    {
        if (string.IsNullOrWhiteSpace(newClaimType) || string.IsNullOrWhiteSpace(newClaimValue) || selectedRole == null)
            return;

        await ExecuteAsync(async () =>
        {
            var role = await RoleManager.FindByIdAsync(selectedRole.Id);
            if (role != null)
            {
                var claim = new Claim(newClaimType.Trim(), newClaimValue.Trim());
                var result = await RoleManager.AddClaimAsync(role, claim);

                if (result.Succeeded)
                {
                    // Refresh the claims list
                    await ViewRoleClaims(selectedRole.Id);
                    CancelAddClaim();
                }
                else
                {
                    SetError($"Failed to add claim: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }, "Adding role claim");
    }

    private async Task RemoveClaim(int claimIndex)
    {
        if (selectedRole == null || claimIndex >= roleClaims.Count)
            return;

        await ExecuteAsync(async () =>
        {
            var role = await RoleManager.FindByIdAsync(selectedRole.Id);
            if (role != null)
            {
                var claimToRemove = roleClaims[claimIndex];
                var claim = new Claim(claimToRemove.ClaimType, claimToRemove.ClaimValue);
                var result = await RoleManager.RemoveClaimAsync(role, claim);

                if (result.Succeeded)
                {
                    // Refresh the claims list
                    await ViewRoleClaims(selectedRole.Id);
                }
                else
                {
                    SetError($"Failed to remove claim: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }, "Removing role claim");
    }

    private void ShowCreateRoleModal()
    {
        showCreateRoleModal = true;
        newRoleName = string.Empty;
    }

    private void CloseCreateRoleModal()
    {
        showCreateRoleModal = false;
        newRoleName = string.Empty;
    }

    private async Task CreateRole()
    {
        if (string.IsNullOrWhiteSpace(newRoleName))
            return;

        await ExecuteAsync(async () =>
        {
            var role = new ApplicationRole
            {
                Name = newRoleName.Trim(),
                IsBackendRole = true,
                TenantId = Domain.Tenants.ValueObjects.TenantId.Empty // System roles have empty tenant
            };

            var result = await RoleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                CloseCreateRoleModal();
                await LoadRoles(); // Refresh the role list
            }
            else
            {
                SetError($"Failed to create role: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }, "Creating role");
    }

    private void EditRole(string roleId)
    {
        // Placeholder for edit functionality
        // In a full implementation, you would show an edit modal
        SetError("Edit functionality is not yet implemented.");
    }

    private async Task DeleteRole(string roleId)
    {
        var role = allRoles.FirstOrDefault(r => r.Id == roleId);
        if (role == null || role.HasUsers)
            return;

        await ExecuteAsync(async () =>
        {
            var roleEntity = await RoleManager.FindByIdAsync(roleId);
            if (roleEntity != null)
            {
                var result = await RoleManager.DeleteAsync(roleEntity);

                if (result.Succeeded)
                {
                    await LoadRoles(); // Refresh the role list
                }
                else
                {
                    SetError($"Failed to delete role: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }, "Deleting role");
    }

    // Helper classes for data binding
    public enum RoleType
    {
        All,
        System,
        Tenant
    }

    public class RoleListItem
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool IsBackendRole { get; set; }
        public Domain.Tenants.ValueObjects.TenantId TenantId { get; set; }
        public string? TenantName { get; set; }
        public int ClaimsCount { get; set; }
        public bool HasUsers { get; set; }
        public bool IsSystemRole => IsBackendRole && TenantId == Domain.Tenants.ValueObjects.TenantId.Empty;
    }

    public class ClaimListItem
    {
        public int Id { get; set; }
        public string ClaimType { get; set; } = string.Empty;
        public string ClaimValue { get; set; } = string.Empty;
    }
}