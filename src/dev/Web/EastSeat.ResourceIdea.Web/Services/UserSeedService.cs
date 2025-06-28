using Microsoft.AspNetCore.Identity;
using EastSeat.ResourceIdea.DataStore.Identity.Entities;
using EastSeat.ResourceIdea.Domain.Users.ValueObjects;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

namespace EastSeat.ResourceIdea.Web.Services;

/// <summary>
/// Service to seed initial user data for the application.
/// </summary>
public class UserSeedService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ILogger<UserSeedService> _logger;

    public UserSeedService(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        ILogger<UserSeedService> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    /// <summary>
    /// Seeds initial roles and admin user.
    /// </summary>
    public async Task SeedAsync()
    {
        try
        {
            // Create default roles
            await CreateRoleIfNotExistsAsync("Admin", TenantId.Create("default-tenant"));
            await CreateRoleIfNotExistsAsync("User", TenantId.Create("default-tenant"));

            // Create default admin user
            await CreateAdminUserIfNotExistsAsync();

            _logger.LogInformation("User seeding completed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while seeding users");
            throw;
        }
    }

    private async Task CreateRoleIfNotExistsAsync(string roleName, TenantId tenantId)
    {
        var roleExists = await _roleManager.RoleExistsAsync(roleName);
        if (!roleExists)
        {
            var role = new ApplicationRole
            {
                Name = roleName,
                NormalizedName = roleName.ToUpperInvariant(),
                TenantId = tenantId,
                IsBackendRole = roleName == "Admin"
            };

            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                _logger.LogInformation("Role '{RoleName}' created successfully", roleName);
            }
            else
            {
                _logger.LogError("Failed to create role '{RoleName}'. Errors: {Errors}",
                    roleName, string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }

    private async Task CreateAdminUserIfNotExistsAsync()
    {
        const string adminUsername = "admin";
        const string adminEmail = "admin@resourceidea.com";
        const string adminPassword = "Admin123!";

        var existingUser = await _userManager.FindByNameAsync(adminUsername);
        if (existingUser == null)
        {
            var adminUser = new ApplicationUser
            {
                UserName = adminUsername,
                NormalizedUserName = adminUsername.ToUpperInvariant(),
                Email = adminEmail,
                NormalizedEmail = adminEmail.ToUpperInvariant(),
                EmailConfirmed = true,
                FirstName = "System",
                LastName = "Administrator",
                ApplicationUserId = ApplicationUserId.Create(Guid.NewGuid().ToString()),
                TenantId = TenantId.Create("default-tenant"),
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                // Add user to Admin role
                await _userManager.AddToRoleAsync(adminUser, "Admin");
                _logger.LogInformation("Admin user created successfully with username: {Username}", adminUsername);
            }
            else
            {
                _logger.LogError("Failed to create admin user. Errors: {Errors}",
                    string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
        else
        {
            _logger.LogInformation("Admin user already exists");
        }
    }
}
