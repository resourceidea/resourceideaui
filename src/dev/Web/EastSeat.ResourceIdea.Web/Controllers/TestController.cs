using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using EastSeat.ResourceIdea.DataStore.Identity.Entities;
using EastSeat.ResourceIdea.Domain.Users.ValueObjects;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

namespace EastSeat.ResourceIdea.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ILogger<TestController> _logger;

        public TestController(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            ILogger<TestController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        [HttpPost("create-test-user")]
        public async Task<IActionResult> CreateTestUser()
        {
            try
            {
                // Create Admin role if it doesn't exist
                var adminRoleExists = await _roleManager.RoleExistsAsync("Admin");
                if (!adminRoleExists)
                {
                    var adminRole = new ApplicationRole
                    {
                        Name = "Admin",
                        NormalizedName = "ADMIN",
                        TenantId = TenantId.Create(Guid.NewGuid()),
                        IsBackendRole = true
                    };
                    var roleResult = await _roleManager.CreateAsync(adminRole);
                    if (!roleResult.Succeeded)
                    {
                        return BadRequest($"Failed to create Admin role: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                    }
                }

                // Create test user
                const string username = "admin";
                const string email = "admin@resourceidea.com";
                const string password = "Admin123!";

                var existingUser = await _userManager.FindByNameAsync(username);
                if (existingUser != null)
                {
                    return Ok("Test user already exists");
                }

                var user = new ApplicationUser
                {
                    UserName = username,
                    NormalizedUserName = username.ToUpperInvariant(),
                    Email = email,
                    NormalizedEmail = email.ToUpperInvariant(),
                    EmailConfirmed = true,
                    FirstName = "Test",
                    LastName = "Admin",
                    ApplicationUserId = ApplicationUserId.Create(Guid.NewGuid().ToString()),
                    TenantId = TenantId.Create(Guid.NewGuid()),
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                    return Ok($"Test user created successfully. Username: {username}, Password: {password}");
                }
                else
                {
                    return BadRequest($"Failed to create user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating test user");
                return StatusCode(500, $"Error creating test user: {ex.Message}");
            }
        }
    }
}
