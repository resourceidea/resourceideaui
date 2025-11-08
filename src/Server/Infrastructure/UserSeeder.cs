using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace EastSeat.ResourceIdea.Server.Infrastructure;

public static class UserSeeder
{
    public static async Task SeedAsync(IServiceProvider services, string usersFilePath, string defaultPassword)
    {
        if (!File.Exists(usersFilePath))
        {
            Console.WriteLine($"[Seed] Users file not found: {usersFilePath}");
            return;
        }

        var json = await File.ReadAllTextAsync(usersFilePath);
        var entries = JsonSerializer.Deserialize<List<SeedUserEntry>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? new();
        // Use provided root service provider directly to avoid isolating in-memory database state in tests
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        var roles = entries.SelectMany(e => e.Roles).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        foreach (var entry in entries)
        {
            if (string.IsNullOrWhiteSpace(entry.Email))
            {
                Console.WriteLine("[Seed] Skipping entry with empty email.");
                continue;
            }
            var existing = await userManager.FindByEmailAsync(entry.Email);
            if (existing == null)
            {
                var user = new IdentityUser { UserName = entry.Email, Email = entry.Email, EmailConfirmed = true };
                var createResult = await userManager.CreateAsync(user, defaultPassword);
                if (!createResult.Succeeded)
                {
                    Console.WriteLine($"[Seed] Failed to create {entry.Email}: {string.Join(',', createResult.Errors.Select(e => e.Description))}");
                    continue;
                }
                existing = user;
                // Created successfully
            }
            if (entry.Roles.Count > 0)
            {
                var currentRoles = await userManager.GetRolesAsync(existing);
                var toAdd = entry.Roles.Except(currentRoles, StringComparer.OrdinalIgnoreCase).ToList();
                if (toAdd.Any())
                {
                    await userManager.AddToRolesAsync(existing, toAdd);
                }
            }
        }
        Console.WriteLine("[Seed] Completed user seeding.");
    }

    private sealed class SeedUserEntry
    {
        [JsonPropertyName("Email")]
        public string Email { get; set; } = string.Empty;
        [JsonPropertyName("Roles")]
        public List<string> Roles { get; set; } = new();
        public string GivenName { get; set; } = string.Empty; // reserved
        public string Surname { get; set; } = string.Empty;   // reserved
    }
}
