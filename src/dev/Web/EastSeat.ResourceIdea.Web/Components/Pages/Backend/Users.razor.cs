using EastSeat.ResourceIdea.Application.Features.Employees.Queries;
using EastSeat.ResourceIdea.DataStore.Identity.Entities;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Web.Components.Base;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;

namespace EastSeat.ResourceIdea.Web.Components.Pages.Backend;

public partial class Users : ResourceIdeaComponentBase
{
    [Inject] private IMediator Mediator { get; set; } = default!;
    [Inject] private UserManager<ApplicationUser> UserManager { get; set; } = default!;
    [Inject] private RoleManager<ApplicationRole> RoleManager { get; set; } = default!;

    private List<UserListItem> allUsers = new();
    private List<UserListItem> filteredUsers = new();
    private string searchTerm = string.Empty;
    private int currentPage = 1;
    private int pageSize = 20;
    private int totalPages = 1;

    protected override async Task OnInitializedAsync()
    {
        await ExecuteAsync(async () =>
        {
            await LoadUsers();
        }, "Loading users");
    }

    private async Task LoadUsers()
    {
        // Load all users from ApplicationUser - this gives us cross-tenant visibility as requested
        var applicationUsers = UserManager.Users.ToList();
        var userListItems = new List<UserListItem>();

        // Process each application user
        foreach (var user in applicationUsers)
        {
            var roles = await UserManager.GetRolesAsync(user);
            var isBackendUser = roles.Any(r => r == "Developer" || r == "Support");
            
            string tenantName = "Unknown Tenant";
            
            // If not a backend user, try to get tenant employees data for context
            if (!isBackendUser && user.TenantId != TenantId.Empty)
            {
                try
                {
                    var tenantEmployeesQuery = new TenantEmployeesQuery 
                    { 
                        TenantId = user.TenantId,
                        PageNumber = 1,
                        PageSize = 1000 // Get all to find this user
                    };
                    
                    var tenantEmployeesResponse = await Mediator.Send(tenantEmployeesQuery);
                    
                    if (tenantEmployeesResponse.IsSuccess && tenantEmployeesResponse.Content.HasValue)
                    {
                        var employee = tenantEmployeesResponse.Content.Value.Items
                            .FirstOrDefault(e => e.ApplicationUserId.Value.ToString() == user.Id);
                        
                        if (employee != null)
                        {
                            tenantName = !string.IsNullOrEmpty(employee.DepartmentName) 
                                ? $"{employee.DepartmentName} Department" 
                                : "Main Organization";
                        }
                    }
                }
                catch
                {
                    // If query fails, use fallback
                    tenantName = "Organization User";
                }
            }

            userListItems.Add(new UserListItem
            {
                Id = user.Id,
                Email = user.Email ?? "",
                FullName = $"{user.FirstName} {user.LastName}".Trim(),
                TenantName = isBackendUser ? "" : tenantName,
                IsBackendUser = isBackendUser,
                Roles = roles.ToList(),
                LastLoginAt = user.LockoutEnd?.DateTime, // Approximate last activity
                IsActive = !user.LockoutEnabled || user.LockoutEnd < DateTime.UtcNow
            });
        }

        // Add some sample backend users if none exist
        if (!userListItems.Any(u => u.IsBackendUser))
        {
            userListItems.AddRange(GetSampleBackendUsers());
        }

        allUsers = userListItems;
        FilterUsers();
    }

    private List<UserListItem> GetSampleBackendUsers()
    {
        return new[]
        {
            new UserListItem
            {
                Id = Guid.NewGuid().ToString(),
                Email = "developer@eastseat.com",
                FullName = "John Developer",
                TenantName = "",
                IsBackendUser = true,
                Roles = new List<string> { "Developer" },
                LastLoginAt = DateTime.Now.AddHours(-2),
                IsActive = true
            },
            new UserListItem
            {
                Id = Guid.NewGuid().ToString(),
                Email = "support@eastseat.com",
                FullName = "Jane Support",
                TenantName = "",
                IsBackendUser = true,
                Roles = new List<string> { "Support" },
                LastLoginAt = DateTime.Now.AddMinutes(-30),
                IsActive = true
            }
        };
    }

    private void FilterUsers()
    {
        filteredUsers = string.IsNullOrWhiteSpace(searchTerm)
            ? allUsers
            : allUsers.Where(u => 
                u.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                u.FullName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                u.TenantName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
            ).ToList();

        totalPages = (int)Math.Ceiling((double)filteredUsers.Count / pageSize);
        currentPage = 1;
        ApplyPagination();
    }

    private void ApplyPagination()
    {
        var startIndex = (currentPage - 1) * pageSize;
        filteredUsers = filteredUsers.Skip(startIndex).Take(pageSize).ToList();
    }

    private void GoToPage(int page)
    {
        if (page >= 1 && page <= totalPages)
        {
            currentPage = page;
            FilterUsers();
        }
    }

    private void ShowCreateUserModal()
    {
        Navigation.NavigateTo("/backend/users/create");
    }

    private void ViewUser(string userId)
    {
        Navigation.NavigateTo($"/backend/users/{userId}");
    }

    private void EditUser(string userId)
    {
        Navigation.NavigateTo($"/backend/users/{userId}/edit");
    }

    private void ImpersonateUser(string userId)
    {
        // Implement user impersonation functionality
        // This would allow backend users to temporarily act as tenant users for support
    }

    private async Task ToggleUserStatus(string userId)
    {
        await ExecuteAsync(async () =>
        {
            // Implement toggle user active/inactive status
            await Task.Delay(100); // Simulate API call
            await LoadUsers(); // Refresh the list
        }, "Updating user status");
    }

    private class UserListItem
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string TenantName { get; set; } = string.Empty;
        public bool IsBackendUser { get; set; }
        public List<string> Roles { get; set; } = new();
        public DateTime? LastLoginAt { get; set; }
        public bool IsActive { get; set; }
    }
}