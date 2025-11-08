using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using EastSeat.ResourceIdea.Server.Data;
using EastSeat.ResourceIdea.Server.Infrastructure;
using Xunit;

namespace ServerTests;

public class UserSeederTests
{
    [Fact]
    public async Task SeedsUsersFromJson()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddDbContext<ApplicationDbContext>(o => o.UseInMemoryDatabase(Guid.NewGuid().ToString()));
        services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            // Relax constraints fully for test reliability
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 4;
            options.User.RequireUniqueEmail = true;
            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@._-";
        }).AddEntityFrameworkStores<ApplicationDbContext>();
        var provider = services.BuildServiceProvider();
        var jsonPath = Path.GetTempFileName();
        // Provide valid JSON with expected 'email' property exactly matching seeder's model
        var entry = new { Email = "seedtest@example.com", Roles = new[] { "Admin" } };
        var json = System.Text.Json.JsonSerializer.Serialize(new[] { entry });
        await File.WriteAllTextAsync(jsonPath, json);
        await UserSeeder.SeedAsync(provider, jsonPath, "Test1");
        // Use root provider's UserManager to verify persistence. (Avoid creating new scope which may
        // produce an isolated in-memory store instance in certain test runner lifecycles.)
        var userManager = provider.GetRequiredService<UserManager<IdentityUser>>();
        var user = await userManager.FindByEmailAsync("seedtest@example.com");
        Assert.NotNull(user);
    }
}
