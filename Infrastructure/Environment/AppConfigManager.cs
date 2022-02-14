using System.Security.Claims;

namespace ResourceIdea.Infrastructure.Environment;

public static class AppConfigManager
{
    public static async void SeedAdminUser(this WebApplication app, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        // Get the credentials to setup the admin user.
        var adminCredentials = EnvironmentConfiguration.GetAdminUserCredentials();
        if (adminCredentials.username is null || adminCredentials.email is null ||
            adminCredentials.password is null) return;

        // Create admin role.
        var adminRoleName = EnvironmentConfiguration.GetAdminRole() ?? "Admin";
        var adminRole = await CreatAdminRoleIfDoesNotExist(roleManager, adminRoleName);

        // Create admin user.
        var adminUser = await userManager.FindByEmailAsync(adminCredentials.email);
        if (adminUser is null)
        {
            adminUser = await CreateAdminUser(userManager, adminCredentials);
        }

        // Assign admin user to the admin role.
        await AssignUserToAdminRole(userManager, adminUser, adminRole);

        // Assign admin permission claims to the admin roles.
        var adminRoleClaims = await roleManager!.GetClaimsAsync(adminRole);
        await AssignClaimsToAdminRole(roleManager, adminRoleClaims, adminRole);
    }

    private static async Task AssignClaimsToAdminRole(RoleManager<IdentityRole> roleManager, IList<Claim> adminRoleClaims,
        IdentityRole adminRole)
    {
        if (!adminRoleClaims.Any(c => c.Type == "Permission" && c.Value == "company.archive"))
        {
            await roleManager.AddClaimAsync(adminRole, new Claim(type: "Permission", value: "company.archive"));
        }

        if (!adminRoleClaims.Any(c => c.Type == "Permission" && c.Value == "company.create"))
        {
            await roleManager.AddClaimAsync(adminRole, new Claim(type: "Permission", value: "company.create"));
        }

        if (!adminRoleClaims.Any(c => c.Type == "Permission" && c.Value == "company.delete"))
        {
            await roleManager.AddClaimAsync(adminRole, new Claim(type: "Permission", value: "company.delete"));
        }

        if (!adminRoleClaims.Any(c => c.Type == "Permission" && c.Value == "company.offline"))
        {
            await roleManager.AddClaimAsync(adminRole, new Claim(type: "Permission", value: "company.offline"));
        }

        if (!adminRoleClaims.Any(c => c.Type == "Permission" && c.Value == "company.online"))
        {
            await roleManager.AddClaimAsync(adminRole, new Claim(type: "Permission", value: "company.online"));
        }

        if (!adminRoleClaims.Any(c => c.Type == "Permission" && c.Value == "company.update"))
        {
            await roleManager.AddClaimAsync(adminRole, new Claim(type: "Permission", value: "company.update"));
        }

        if (!adminRoleClaims.Any(c => c.Type == "Permission" && c.Value == "company.view"))
        {
            await roleManager.AddClaimAsync(adminRole, new Claim(type: "Permission", value: "company.view"));
        }
    }

    private static async Task AssignUserToAdminRole(UserManager<ApplicationUser> userManager, ApplicationUser adminUser,
        IdentityRole adminRole)
    {
        if (adminUser is null || adminRole is null) return;
        
        var isUserAssignedToAdminRole = await userManager.IsInRoleAsync(adminUser, adminRole.Name);
        if (!isUserAssignedToAdminRole)
        {
            await userManager.AddToRoleAsync(adminUser, adminRole.Name);
        }
    }

    private static async Task<IdentityRole> CreatAdminRoleIfDoesNotExist(RoleManager<IdentityRole>? roleManager, string adminRoleName)
    {
        var adminRole = await roleManager!.FindByNameAsync(adminRoleName);
        if (adminRole is not null) return adminRole;
        adminRole = new IdentityRole(adminRoleName);
        await roleManager.CreateAsync(adminRole);

        return adminRole;
    }

    private static async Task<ApplicationUser> CreateAdminUser(UserManager<ApplicationUser> userManager,
        (string? username, string? email, string? password, string? firstname, string? lastname) adminCredentials)
    {
        ApplicationUser adminUser;
        adminUser = new ApplicationUser
        {
            Email = adminCredentials.email,
            UserName = adminCredentials.username,
            FirstName = adminCredentials.firstname,
            LastName = adminCredentials.lastname,
            EmailConfirmed = true
        };
        await userManager.CreateAsync(adminUser, adminCredentials.password);
        return adminUser;
    }
}