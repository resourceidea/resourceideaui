using System.Security.Claims;

namespace ResourceIdea.Web.Infrastructure.Environment;

public static class AppConfigManager
{
    public static async void SeedAdminUser(this WebApplication app, UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        var adminCredentials = (
            username: app.Configuration["Admin:Username"],
            email: app.Configuration["Admin:Email"],
            password: app.Configuration["Admin:Password"],
            firstname: app.Configuration["Admin:Firstname"],
            lastname: app.Configuration["Admin:Lastname"],
            companyCode: app.Configuration["Admin:CompanyCode"]
        );
        if (adminCredentials.username is null ||
            adminCredentials.email is null ||
            adminCredentials.password is null)
        {
            return;
        }

        // Create admin role.
        var adminRoleName = app.Configuration["Admin:Role"] ?? "Admin";
        var adminRole = await CreatAdminRoleIfDoesNotExist(roleManager, adminRoleName);

        // Create admin user.
        var adminUser = await userManager.FindByEmailAsync(adminCredentials.email) ??
                        await CreateAdminUser(userManager, adminCredentials);

        // Assign admin user to the admin role.
        await AssignUserToAdminRole(userManager, adminUser, adminRole);

        // Assign admin permission claims to the admin roles.
        var adminRoleClaims = await roleManager.GetClaimsAsync(adminRole);
        await AssignClaimsToAdminRole(roleManager, adminRoleClaims, adminRole);
    }

    private static async System.Threading.Tasks.Task AssignClaimsToAdminRole(RoleManager<IdentityRole> roleManager,
        IList<Claim> adminRoleClaims,
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

    private static async System.Threading.Tasks.Task AssignUserToAdminRole(UserManager<ApplicationUser> userManager, ApplicationUser adminUser,
        IdentityRole adminRole)
    {
        var isUserAssignedToAdminRole = await userManager.IsInRoleAsync(adminUser, adminRole.Name ?? "User");
        if (!isUserAssignedToAdminRole)
        {
            await userManager.AddToRoleAsync(adminUser, adminRole.Name ?? "User");
        }
    }

    private static async Task<IdentityRole> CreatAdminRoleIfDoesNotExist(RoleManager<IdentityRole>? roleManager,
        string adminRoleName)
    {
        var adminRole = await roleManager!.FindByNameAsync(adminRoleName);
        if (adminRole is not null) return adminRole;
        adminRole = new IdentityRole(adminRoleName);
        await roleManager.CreateAsync(adminRole);

        return adminRole;
    }

    private static async Task<ApplicationUser> CreateAdminUser(UserManager<ApplicationUser> userManager,
        (string? username, string? email, string? password, string? firstname, string? lastname, string? companyCode)
            adminCredentials)
    {
        var adminUser = new ApplicationUser
        {
            Email = adminCredentials.email,
            UserName = adminCredentials.username,
            FirstName = adminCredentials.firstname,
            LastName = adminCredentials.lastname,
            EmailConfirmed = true,
            CompanyCode = adminCredentials.companyCode
        };
        await userManager.CreateAsync(adminUser, adminCredentials.password!);
        return adminUser;
    }
}