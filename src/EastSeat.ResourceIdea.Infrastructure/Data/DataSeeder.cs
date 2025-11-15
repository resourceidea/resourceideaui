using EastSeat.ResourceIdea.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace EastSeat.ResourceIdea.Infrastructure.Data;

/// <summary>
/// Seeds initial roles and admin user
/// </summary>
public class DataSeeder
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ILogger<DataSeeder> _logger;

    public DataSeeder(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        ILogger<DataSeeder> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        await SeedRolesAsync();
        await SeedAdminUserAsync();
    }

    private async Task SeedRolesAsync()
    {
        var roles = new[]
        {
            new ApplicationRole
            {
                Name = RoleNames.Admin,
                SeniorityLevel = 10,
                Description = "System Administrator"
            },
            new ApplicationRole
            {
                Name = RoleNames.Partner,
                SeniorityLevel = 8,
                Description = "Engagement Partner"
            },
            new ApplicationRole
            {
                Name = RoleNames.Manager,
                SeniorityLevel = 5,
                Description = "Engagement Manager"
            },
            new ApplicationRole
            {
                Name = RoleNames.Staff,
                SeniorityLevel = 2,
                Description = "Staff Member"
            }
        };

        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role.Name!))
            {
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Created role: {RoleName}", role.Name);
                }
                else
                {
                    _logger.LogError("Failed to create role {RoleName}: {Errors}",
                        role.Name, string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
        }
    }

    private async Task SeedAdminUserAsync()
    {
        const string adminEmail = "admin@eastseat.com";

        var existingUser = await _userManager.FindByEmailAsync(adminEmail);
        if (existingUser != null)
        {
            _logger.LogInformation("Admin user already exists");
            return;
        }

        var adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true,
            DisplayName = "System Administrator",
            StaffCode = "ADMIN001",
            IsActive = true
        };

        var password = Environment.GetEnvironmentVariable("ADMIN_PASSWORD") ?? "Admin@123";
        var result = await _userManager.CreateAsync(adminUser, password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(adminUser, RoleNames.Admin);
            _logger.LogInformation("Admin user created successfully");
        }
        else
        {
            _logger.LogError("Failed to create admin user: {Errors}",
                string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}
